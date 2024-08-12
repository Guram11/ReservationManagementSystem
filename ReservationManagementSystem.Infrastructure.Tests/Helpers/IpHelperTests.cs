using ReservationManagementSystem.Infrastructure.Identity.Helpers;

namespace ReservationManagementSystem.Infrastructure.Tests.Helpers;

public class IpHelperTests
{
    [Fact]
    public void GetIpAddress_ShouldReturnNonEmptyString()
    {
        // Act
        var ipAddress = IpHelper.GetIpAddress();

        // Assert
        Assert.False(string.IsNullOrEmpty(ipAddress));
        Assert.Matches(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$", ipAddress);
    }

    [Fact]
    public void GetIpAddress_ShouldReturnValidIpAddress()
    {
        // Act
        var ipAddress = IpHelper.GetIpAddress();

        // Assert
        Assert.False(string.IsNullOrEmpty(ipAddress));
        var ipParts = ipAddress.Split('.');
        Assert.Equal(4, ipParts.Length);
        foreach (var part in ipParts)
        {
            Assert.InRange(int.Parse(part), 0, 255);
        }
    }
}
