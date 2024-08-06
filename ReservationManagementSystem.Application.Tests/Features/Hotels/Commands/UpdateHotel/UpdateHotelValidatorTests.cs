using FluentValidation.TestHelper;
using ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels.Commands.Commands.UpdateHotel;

public class UpdateHotelValidatorTests
{
    private readonly UpdateHotelValidator _validator = new UpdateHotelValidator();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var request = new UpdateHotelRequest(Guid.NewGuid() ,string.Empty);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var request = new UpdateHotelRequest(Guid.NewGuid(), "Valid Hotel Name");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
