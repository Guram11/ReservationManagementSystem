using FluentValidation.TestHelper;
using ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;

namespace ReservationManagementSystem.Application.Tests.Features.Hotels;

public class UpdateHotelValidatorTests
{
    private readonly UpdateHotelValidator _validator = new UpdateHotelValidator();

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        var request = new UpdateHotelRequest(Guid.NewGuid(), string.Empty);
        var result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ShouldNotHaveError_WhenNameIsValid()
    {
        var request = new UpdateHotelRequest(Guid.NewGuid(), "Valid Hotel Name");
        var result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
