using CsvHelper.Configuration.Attributes;

namespace GraphAndTable.Models
{
    public class DataModel
    {
        [Ignore]
        public int Id { get; set; }
        public string Date { get; set; }
        [Name("Market Price EX1")]
        public string MarketPriceEX1 { get; set; }
    }
    public class DetailModel
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double AveragePrice { get; set; }
        public string MostExpensiveHour { get; set; }
        public double HighestHourlySum { get; set; }


    }
}
