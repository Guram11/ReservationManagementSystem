using FluentValidation.TestHelper;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels;

public class CreateHotelValidatorTests
{
    private readonly CreateHotelValidator _validator = new CreateHotelValidator();

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        var request = new CreateHotelRequest(string.Empty);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldNotHaveError_WhenNameIsValid()
    {
        var request = new CreateHotelRequest("Valid Hotel Name");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
