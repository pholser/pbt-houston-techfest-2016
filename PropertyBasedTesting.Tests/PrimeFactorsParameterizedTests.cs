using System.Collections.Generic;
using Xunit;

namespace PropertyBasedTesting.Tests
{
    public class PrimeFactorsParameterizedTests
    {
        [Theory]
        [MemberData(nameof(NumbersAndFactors))]
        public void VerifyFactors(int n, List<int> factors)
        {
            Assert.Equal(factors, PrimeFactors.Of(n));
        }

        public static IEnumerable<object[]> NumbersAndFactors()
        {
            yield return new object[]
            {
                1, new List<int>()
            };
            yield return new object[]
            {
                2, new List<int> { 2 }
            };
            yield return new object[]
            {
                3, new List<int> { 3 }
            };
            yield return new object[]
            {
                4, new List<int> { 2, 2 }
            };
            yield return new object[]
            {
                5, new List<int> { 5 }
            };
            yield return new object[]
            {
                6, new List<int> { 2, 3 }
            };
            yield return new object[]
            {
                7, new List<int> { 7 }
            };
            yield return new object[]
            {
                8, new List<int> { 2, 2, 2 }
            };
            yield return new object[]
            {
                9, new List<int> { 3, 3 }
            };
            yield return new object[]
            {
                10, new List<int> { 2, 5 }
            };
        }
    }
}
