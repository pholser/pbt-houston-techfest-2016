using System;
using Xunit;

namespace PropertyBasedTesting.Tests
{
    public class LineItemTests
    {
        [Fact]
        public void ComputesAmount()
        {
            Assert.Equal(
                new Money(35.20M),
                new LineItem("item1", new Money(17.60M), 2).Amount);
        }

        [Fact]
        public void RejectsNegativePrices()
        {
            Assert.Throws<ArgumentException>(
                () => new LineItem("", -0.01M, 2));
        }

        [Fact]
        public void RejectsNegativeQuantities()
        {
            Assert.Throws<ArgumentException>(
                () => new LineItem("", 0.01M, -1));
        }
    }
}
