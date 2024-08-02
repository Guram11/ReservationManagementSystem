using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ReservationManagementSystem.Infrastructure.Identity.Helpers;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.DTOs.Email;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;
using Microsoft.Extensions.Options;
using ReservationManagementSystem.Infrastructure.Common;

namespace Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MailSettings _mailSettings;
        private readonly JWTSettings _jwtSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings,
            IOptions<MailSettings> mailSettings, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _mailSettings = mailSettings.Value;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Result<AuthenticationResponse>.Failure(AccountServiceErrors.UserNotFound(request.Email));
            }

            if (user.UserName is null || user.Email is null)
            {
                return Result<AuthenticationResponse>.Failure(AccountServiceErrors.InvalidCredentials(request.Email));
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Result<AuthenticationResponse>.Failure(AccountServiceErrors.InvalidCredentials(request.Email));
            }

            if (!user.EmailConfirmed)
            {
                return Result<AuthenticationResponse>.Failure(AccountServiceErrors.EmailNotConfirmed(request.Email));
            }

            try
            {
                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                var refreshToken = GenerateRefreshToken(ipAddress);

                var response = new AuthenticationResponse
                {
                    Id = user.Id,
                    JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = rolesList.ToList(),
                    IsVerified = user.EmailConfirmed,
                    RefreshToken = refreshToken.Token
                };

                return Result<AuthenticationResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<AuthenticationResponse>.Failure(AccountServiceErrors.TokenGenerationError(ex.Message));
            }
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return Result<string>.Failure(AccountServiceErrors.UsernameTaken(request.UserName));
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                return Result<string>.Failure(AccountServiceErrors.EmailRegistered(request.Email));
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(new Error("User.CreationFailed", $"User creation failed: {errors}"));
            }

            await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
            var verificationUri = await SendVerificationEmail(user, origin);

            try
            {
                await _emailService.SendAsync(new EmailRequest
                {
                    From = _mailSettings.EmailFrom,
                    To = user.Email,
                    Body = $"Please confirm your account by visiting this URL {verificationUri}",
                    Subject = "Confirm Registration"
                });

                return Result<string>.Success(user.Id);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(EmailServiceErrors.EmailNotSent(ex.Message));
            }
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            if (user.UserName is null || user.Email is null)
            {
                throw new Exception($"Invalid Credentials for '{user.Email}'.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            var randomBytes = new byte[40];
            RandomNumberGenerator.Fill(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

            return verificationUri;
        }

        public async Task<Result<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.Email is null)
            {
                return Result<string>.Failure(AccountServiceErrors.UserNotFound(userId));
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Result<string>.Success(user.Id);
            }
            else
            {
                return Result<string>.Failure(AccountServiceErrors.EmailConfirmationFailed(user.Email));
            }
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<Result<string>> ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            if (account is null)
            {
                return Result<string>.Failure(AccountServiceErrors.UserNotFound(model.Email));
            };

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));

            var emailRequest = new EmailRequest
            {
                Body = $"Your reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password"
            };

            try
            {
                await _emailService.SendAsync(emailRequest);
                return Result<string>.Success("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(EmailServiceErrors.EmailNotSent(ex.Message));
            }
        }

        public async Task<Result<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null)
            {
                return Result<string>.Failure(AccountServiceErrors.UserNotFound(model.Email));
            }

            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);

            if (result.Succeeded)
            {
                return Result<string>.Success(model.Email);
            }
            else
            {
                return Result<string>.Failure(AccountServiceErrors.PasswordResetFailed(model.Email));
            }
        }
    }
}