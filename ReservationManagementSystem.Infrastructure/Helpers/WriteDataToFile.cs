using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Infrastructure.Helpers;

internal class WriteDataToFile
{
    public static void WriteCurrenciesToFile(CurrencyRatesResponse data, DateTime date)
    {
        try
        {
            if (data.Rates is null)
            {
                throw new Exception("Data does not contain currency rates!");
            }

            var filePath = $"currency-rates-{date:yyyy-MM-dd}.txt";

            using StreamWriter writer = new(filePath);

            var amount = data.MainCurrency == "AMD" ? "1" : "";
            foreach (KeyValuePair<string, decimal> kvp in data.Rates)
            {

                writer.WriteLine($"1 {amount}{kvp.Key} = {kvp.Value} {data.MainCurrency}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing data to file: {ex.Message}");
        }
    }
}
