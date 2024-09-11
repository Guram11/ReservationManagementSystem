//using Microsoft.Data.SqlClient;

//namespace ReservationManagementSystem.FunctionalTests;

//public class FunctionalTests : IClassFixture<DatabaseFixture>
//{
//    private readonly DatabaseFixture _fixture;

//    public FunctionalTests(DatabaseFixture fixture)
//    {
//        _fixture = fixture;
//    }

//    [Fact]
//    public async Task TestDatabaseConnection()
//    {
//        var connectionString = "Server=localhost;Database=ReservationSystemDb;Trusted_Connection=True;TrustServerCertificate=True";

//        using (var connection = new SqlConnection(connectionString))
//        {
//            await connection.OpenAsync();
//            Assert.True(connection.State == System.Data.ConnectionState.Open);
//        }
//    }
//}
