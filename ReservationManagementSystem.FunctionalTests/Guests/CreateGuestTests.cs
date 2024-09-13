using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.FunctionalTests.Abstractions;
using System.Net.Http.Json;

namespace ReservationManagementSystem.FunctionalTests.Guests;

public class GuestsControllerTests()
{
    [Fact]
    public async Task Create_ReturnsOkResultWithCreatedGuest_WhenRequestIsSuccessful()
    {
        // Arrange
        var application = new CustomWebApiFactory();
        var client = application.CreateClient();

        var request = new CreateGuestRequest("guest1@mail.com", "guest1", "guest1", "0123456789", Guid.NewGuid());

        // Act
        var response = await client.PostAsJsonAsync("/api/Guests", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var guestResponse = await response.Content.ReadFromJsonAsync<Result<GuestResponse>>();
        guestResponse?.Data.FirstName.Should().Contain("guest1");
    }
}