using Xunit;

namespace PropertyBasedTesting.Tests
{
    public class MoneyTests
    {
        [Fact]
        public void RoundsDownToNearestCent()
        {
            Assert.Equal(new Money(2.50M), new Money(2.4999M));
        }

        [Fact]
        public void AdditionWorks()
        {
            Assert.Equal(
                new Money(5.46M),
                new Money(3.92M) + new Money(1.54M));
        }

        [Fact]
        public void SubtractionWorks()
        {
            Assert.Equal(
                new Money(4.03M), 
                new Money(5.58M) - new Money(1.55M));
        }

        [Fact]
        public void MultiplicationWorks()
        {
            Assert.Equal(
                new Money(45.28M),
                new Money(22.64M) * 2);
        }

        [Fact]
        public void ConvertsImplicitlyFromDecimal()
        {
            Money converted = 5;

            Assert.Equal(new Money(5), converted);
        }

        //        [Property]
        //        public void AdditionIsCommutative(Money first, Money second)
        //        {
        //            Assert.Equal(first + second, second + first);
        //        }
    }
}
