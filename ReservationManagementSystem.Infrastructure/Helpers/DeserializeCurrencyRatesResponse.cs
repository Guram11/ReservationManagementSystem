using HtmlAgilityPack;
using ReservationManagementSystem.Domain.Settings;
using System.Xml.Linq;

namespace ReservationManagementSystem.Infrastructure.Helpers;

public static class DeserializeCurrencyRatesResponse
{
    public static CurrencyRatesResponse DeserializeXmlResponse(string data)
    {
        var currencyRates = new Dictionary<string, decimal>();

        var xDoc = XDocument.Parse(data);

        var descriptionElement = xDoc?.Descendants("item")?.FirstOrDefault()?.Descendants("description").FirstOrDefault();

        if (descriptionElement != null)
        {
            var cdataContent = descriptionElement.Value;

            var startIndex = cdataContent.IndexOf("<table");
            var endIndex = cdataContent.IndexOf("</table>") + "</table>".Length;

            if (startIndex >= 0 && endIndex > startIndex)
            {
                var tableContent = cdataContent.Substring(startIndex, endIndex - startIndex);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(tableContent);

                foreach (var row in htmlDoc.DocumentNode.SelectNodes("//tr"))
                {
                    var cells = row.SelectNodes("td").ToList();
                    if (cells.Count >= 3)
                    {
                        var currencyCode = cells[0].InnerText.Trim();
                        var rate = decimal.Parse(cells[2].InnerText.Trim());

                        currencyRates[currencyCode] = rate;
                    }
                }
            }
        }

        return new CurrencyRatesResponse
        {
            MainCurrency = "GEL",
            Rates = currencyRates
        };
    }
}
