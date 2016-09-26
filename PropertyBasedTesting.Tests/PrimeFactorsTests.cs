using System.Collections.Generic;
using Xunit;

namespace PropertyBasedTesting.Tests
{
    public class PrimeFactorsTests
    {
        [Fact]
        public void One()
        {
            Assert.Equal(
                new List<int>(),
                PrimeFactors.Of(1));
        }

        [Fact]
        public void Two()
        {
            Assert.Equal(
                new List<int> { 2 },
                PrimeFactors.Of(2));
        }

        [Fact]
        public void Three()
        {
            Assert.Equal(
                new List<int> { 3 },
                PrimeFactors.Of(3));
        }

        [Fact]
        public void Four()
        {
            Assert.Equal(
                new List<int> { 2, 2 },
                PrimeFactors.Of(4));
        }

        [Fact]
        public void Five()
        {
            Assert.Equal(
                new List<int> { 5 },
                PrimeFactors.Of(5));
        }

        [Fact]
        public void Six()
        {
            Assert.Equal(
                new List<int> { 2, 3 },
                PrimeFactors.Of(6));
        }

        [Fact]
        public void Seven()
        {
            Assert.Equal(
                new List<int> { 7 },
                PrimeFactors.Of(7));
        }

        [Fact]
        public void Eight()
        {
            Assert.Equal(
                new List<int> { 2, 2, 2 },
                PrimeFactors.Of(8));
        }

        [Fact]
        public void Nine()
        {
            Assert.Equal(
                new List<int> { 3, 3 },
                PrimeFactors.Of(9));
        }

        [Fact]
        public void Ten()
        {
            Assert.Equal(
                new List<int> { 2, 5 },
                PrimeFactors.Of(10));
        }
    }
}
