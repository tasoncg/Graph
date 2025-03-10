using GraphAndTable.Models;
using GraphAndTable.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;


namespace GraphAndTable.Tests
{
    public class DetailTests
    {
        private readonly MarketDataService _service;
        public DetailTests()
        {
            _service = new MarketDataService();
        }

        [Fact]
        public void GetMaxPrice_ShouldReturnCorrectMax()
        {
            // Arrange
            var data = new List<double>() { 50.5, 60.2, 55.8 };
       

            // Act
            double result = _service.GetMaxPrice(data);

            // Assert
            Assert.Equal(60.2, result);
        }

    }
}
