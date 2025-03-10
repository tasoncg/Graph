using GraphAndTable.Models;


namespace GraphAndTable.Service
{
    public class MarketDataService
    {
        public double GetMaxPrice(List<double> marketData)
        {
            if (marketData == null || marketData.Count == 0)
                throw new ArgumentException("Market data cannot be null or empty");

            return marketData.Max();
        }
    }
}
