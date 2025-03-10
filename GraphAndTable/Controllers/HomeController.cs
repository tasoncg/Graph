using GraphAndTable.Models;
using GraphAndTable.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace GraphAndTable.Controllers
{
    public class HomeController : Controller
    {
        private readonly CsvService _csvService;
        private readonly MarketDataService _marketDataService;

        public HomeController()
        {
            _csvService = new CsvService();
            _marketDataService = new MarketDataService();
        }

        public IActionResult Index()
        {
            var data = _csvService.ReadCsv("sampleSheet.csv");
            return View(data);
        }
        public IActionResult Detail()
        {
            var marketData = _csvService.ReadCsv("sampleSheet.csv");

            if (marketData == null || marketData.Count == 0)
                throw new ArgumentException("Market data is empty");

            // Convert MarketPriceEX1 to double safely
            var priceList = marketData
                .Select(d => double.TryParse(d.MarketPriceEX1, NumberStyles.Any, CultureInfo.InvariantCulture, out double price) ? price : 0)
                .ToList();

            double minPrice = priceList.Min();
            double maxPrice = _marketDataService.GetMaxPrice(priceList);
            double averagePrice = priceList.Average();
            string[] dateFormats = {
    "d/M/yyyy H:mm",    // Handles "10/1/2017 0:30"
    "d/M/yyyy HH:mm",   // Handles "10/1/2017 00:30"
    "dd/MM/yyyy H:mm",  // Handles "10/01/2017 0:30"
    "dd/MM/yyyy HH:mm", // Handles "10/01/2017 00:30"
    "d/M/yyyy",         // Handles "10/1/2017" (date-only)
    "dd/MM/yyyy"        // Handles "10/01/2017" (date-only)
};

            // Sort by Date
            var sortedData = marketData
     .OrderBy(d =>
     {
         if (DateTime.TryParseExact(d.Date, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
         {
             return parsedDate;
         }
         else
         {
             return DateTime.MinValue; // Assign a default value in case of invalid format
         }
     })
     .ToList();

            string mostExpensiveHour = "";
            double highestHourlySum = double.MinValue;

            // Iterate over data and find max sum of consecutive half-hourly values
            for (int i = 0; i < sortedData.Count - 1; i++)
            {
                if (DateTime.TryParseExact(sortedData[i].Date, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime currentTime))
                {
                    // Success - currentTime contains the parsed date
                }
                else
                {
                    // Handle parsing failure
                    Console.WriteLine($"Failed to parse date: {sortedData[i].Date}");
                }

                DateTime nextTime;
                if (i + 1 < sortedData.Count &&
                    DateTime.TryParseExact(sortedData[i + 1].Date, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out nextTime))
                {
                    // Successfully parsed nextTime
                }
                else
                {
                    // Handle parsing failure (e.g., set a default value or log an error)
                    Console.WriteLine($"Failed to parse date: {sortedData[i + 1].Date}");
                    nextTime = DateTime.MinValue; // Assign a fallback value
                }



                if ((nextTime - currentTime).TotalMinutes == 30) // Ensure consecutive half-hourly slots
                {
                    double currentPrice = double.TryParse(sortedData[i].MarketPriceEX1, NumberStyles.Any, CultureInfo.InvariantCulture, out double cp) ? cp : 0;
                    double nextPrice = double.TryParse(sortedData[i + 1].MarketPriceEX1, NumberStyles.Any, CultureInfo.InvariantCulture, out double np) ? np : 0;

                    double sum = currentPrice + nextPrice;
                    if (sum > highestHourlySum)
                    {
                        highestHourlySum = sum;
                        mostExpensiveHour = $"{sortedData[i].Date} - {sortedData[i + 1].Date}";
                    }
                }
            }

            DetailModel result = new DetailModel()
            {
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                AveragePrice = averagePrice,
                MostExpensiveHour = mostExpensiveHour,
                HighestHourlySum = highestHourlySum
            };
            return View(result);
        }


    }
}
