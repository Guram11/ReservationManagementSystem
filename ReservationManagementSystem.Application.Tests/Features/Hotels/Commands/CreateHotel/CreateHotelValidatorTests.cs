using FluentValidation.TestHelper;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels.Commands.CreateHotel;

public class CreateHotelValidatorTests
{
    private readonly CreateHotelValidator _validator = new CreateHotelValidator();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var request = new CreateHotelRequest(string.Empty);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var request = new CreateHotelRequest("Valid Hotel Name");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
