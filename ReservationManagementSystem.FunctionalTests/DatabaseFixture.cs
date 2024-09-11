using Microsoft.Data.SqlClient;

namespace ReservationManagementSystem.FunctionalTests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly string _connectionString = "Server=localhost;Database=ReservationSystemDb;Trusted_Connection=True;TrustServerCertificate=True";

    public async Task InitializeAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(20));

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("CREATE DATABASE IF NOT EXISTS ReservationManagementDb;", connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
