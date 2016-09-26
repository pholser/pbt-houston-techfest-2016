using System.Collections.Generic;
using Xunit;

namespace PropertyBasedTesting.Tests
{
    public class MoneyAllocatorTests
    {
        private readonly MoneyAllocator _allocator;

        public MoneyAllocatorTests()
        {
            _allocator = new MoneyAllocator();
        }

        [Fact]
        public void NoItems()
        {
            var allocations =
                _allocator.Allocate(
                    1,
                    new List<LineItem>());

            Assert.Equal(0, allocations.Count);
        }

        [Fact]
        public void SingleItemDividingEvenly()
        {
            var allocations =
                _allocator.Allocate(
                    1,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.65M, 2)
                    });

            Assert.Equal(1, allocations.Count);
            AssertItem(allocations[0], "foo", 0.50M, 2);
        }

        [Fact]
        public void SumOfItemAmountsEqualToAmountToDivide()
        {
            var allocations =
                _allocator.Allocate(
                    1.15M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.65M, 1),
                        new LineItem("bar", 0.50M, 1)
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.65M, 1);
            AssertItem(allocations[1], "bar", 0.50M, 1);
        }

        [Fact]
        public void SumOfItemAmountsEqualToAmountToDivideDifferentOrder()
        {
            var allocations =
                _allocator.Allocate(
                    1.15M,
                    new List<LineItem>
                    {
                        new LineItem("bar", 0.50M, 1),
                        new LineItem("foo", 0.65M, 1)
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "bar", 0.50M, 1);
            AssertItem(allocations[1], "foo", 0.65M, 1);
        }

        [Fact]
        public void MultipleItemsDividingEvenly()
        {
            var allocations =
                _allocator.Allocate(
                    1,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.65M, 1),
                        new LineItem("bar", 0.65M, 1)
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.50M, 1);
            AssertItem(allocations[1], "bar", 0.50M, 1);
        }

        [Fact]
        public void SingleItemNotDividingEvenly()
        {
            var allocations =
                _allocator.Allocate(
                    1.01M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.65M, 2),
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.51M, 1);
            AssertItem(allocations[1], "foo", 0.50M, 1);
        }

        [Fact]
        public void SingleItemOfGreatQuantity()
        {
            var allocations =
                _allocator.Allocate(
                    38,
                    new List<LineItem>
                    {
                        new LineItem("bubble wrap", 1.29M, 41),
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "bubble wrap", 0.93M, 28);
            AssertItem(allocations[1], "bubble wrap", 0.92M, 13);
        }

        [Fact]
        public void QuantityCanExceedAvailablePennies()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.05M, 501),
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.01M, 500);
            AssertItem(allocations[1], "foo", 0, 1);
        }

        [Fact]
        public void QuantityCanExceedAvailablePenniesByMoreThanDouble()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.05M, 1013),
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.01M, 500);
            AssertItem(allocations[1], "foo", 0, 513);
        }

        [Fact]
        public void QuantityMatchingAvailablePennies()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.05M, 500),
                    });

            Assert.Equal(1, allocations.Count);
            AssertItem(allocations[0], "foo", 0.01M, 500);
        }

        [Fact]
        public void QuantitySlightlyFewerThanAvailablePennies()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.05M, 499),
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.02M, 1);
            AssertItem(allocations[1], "foo", 0.01M, 498);
        }

        [Fact]
        public void QuantityInExcessOfAvailablePenniesAndExtraItem()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0.99M, 512),
                        new LineItem("bar", 3.99M, 1),
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "bar", 0.03M, 1);
            AssertItem(allocations[1], "foo", 0.01M, 497);
            AssertItem(allocations[2], "foo", 0, 15);
        }

        [Fact]
        public void QuantityInExcessOfAvailablePenniesAndExtraItemDifferentOrder()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("bar", 3.99M, 1),
                        new LineItem("foo", 0.99M, 512)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "bar", 0.04M, 1);
            AssertItem(allocations[1], "foo", 0.01M, 496);
            AssertItem(allocations[2], "foo", 0, 16);
        }

        [Fact]
        public void PennyToOneItemInQuantity()
        {
            var allocations =
                _allocator.Allocate(
                    0.01M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 34.98M, 5)
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 0.01M, 1);
            AssertItem(allocations[1], "foo", 0, 4);
        }

        [Fact]
        public void PennyToTwoItems()
        {
            var allocations =
                _allocator.Allocate(
                    0.01M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 34.98M, 2),
                        new LineItem("bar", 89.99M, 3)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "bar", 0, 3);
            AssertItem(allocations[1], "foo", 0.01M, 1);
            AssertItem(allocations[2], "foo", 0, 1);
        }

        [Fact]
        public void SimilarAmountsFavoringLargerQuantity()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 4.99M, 1),
                        new LineItem("bar", 0.01M, 501)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 2.50M, 1);
            AssertItem(allocations[1], "bar", 0.01M, 250);
            AssertItem(allocations[2], "bar", 0, 251);
        }

        [Fact]
        public void EvenDistributionWhenAmountsSame()
        {
            var allocations =
                _allocator.Allocate(
                    9.98M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 4.99M, 1),
                        new LineItem("bar", 0.01M, 499)
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "foo", 4.99M, 1);
            AssertItem(allocations[1], "bar", 0.01M, 499);
        }

        [Fact]
        public void FavoringSlightlyLargerAmount()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 5.02M, 1),
                        new LineItem("bar", 0.01M, 501)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 2.51M, 1);
            AssertItem(allocations[1], "bar", 0.01M, 249);
            AssertItem(allocations[2], "bar", 0, 252);
        }

        [Fact]
        public void EqualItemsByAmountButQuantityItemExceedsPennies()
        {
            var allocations =
                _allocator.Allocate(
                    5,
                    new List<LineItem>
                    {
                        new LineItem("foo", 5.01M, 1),
                        new LineItem("bar", 0.01M, 501)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 2.51M, 1);
            AssertItem(allocations[1], "bar", 0.01M, 249);
            AssertItem(allocations[2], "bar", 0, 252);
        }

        [Fact]
        public void EqualItemsByAmountAndPenniesBarelyExceedsQuantity()
        {
            var allocations =
                _allocator.Allocate(
                    5.02M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 5.01M, 1),
                        new LineItem("bar", 0.01M, 501)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 2.52M, 1);
            AssertItem(allocations[1], "bar", 0.01M, 250);
            AssertItem(allocations[2], "bar", 0, 251);
        }

        [Fact]
        public void MetroStarterUnit()
        {
            var allocations =
                _allocator.Allocate(
                    109,
                    new List<LineItem>
                    {
                        new LineItem("shelf", 23.99M, 4),
                        new LineItem("pole", 10.99M, 4)
                    });

            Assert.Equal(2, allocations.Count);
            AssertItem(allocations[0], "shelf", 18.69M, 4);
            AssertItem(allocations[1], "pole", 8.56M, 4);
        }

        [Fact]
        public void IgnoresZeroDollarItemsWhenDistributingRemainder()
        {
            var allocations =
                _allocator.Allocate(
                    1.01M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0, 1),
                        new LineItem("bar", 0.5M, 1),
                        new LineItem("baz", 0.5M, 1)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "baz", 0.5M, 1);
            AssertItem(allocations[1], "foo", 0, 1);
            AssertItem(allocations[2], "bar", 0.51M, 1);
        }

        [Fact]
        public void EvenDistributionWithZeroPriceItemsPresent()
        {
            var allocations =
                _allocator.Allocate(
                    16,
                    new List<LineItem>
                    {
                        new LineItem("foo", 10, 1),
                        new LineItem("bar", 3, 2),
                        new LineItem("baz", 0, 1)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 10, 1);
            AssertItem(allocations[1], "bar", 3, 2);
            AssertItem(allocations[2], "baz", 0, 1);
        }

        [Fact]
        public void UnevenDistributionWithZeroPriceItemsPresent()
        {
            var allocations =
                _allocator.Allocate(
                    15.01M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 10, 1),
                        new LineItem("bar", 3, 2),
                        new LineItem("baz", 0, 34)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "bar", 2.81M, 2);
            AssertItem(allocations[1], "baz", 0, 34);
            AssertItem(allocations[2], "foo", 9.39M, 1);
        }

        [Fact]
        public void EvenDistributionWithZeroQuantityItemsPresent()
        {
            var allocations =
                _allocator.Allocate(
                    16,
                    new List<LineItem>
                    {
                        new LineItem("foo", 10, 1),
                        new LineItem("bar", 3, 2),
                        new LineItem("baz", 0.5M, 0)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 10, 1);
            AssertItem(allocations[1], "bar", 3, 2);
            AssertItem(allocations[2], "baz", 0, 0);
        }

        [Fact]
        public void UnevenDistributionWithZeroQuantityItemsPresent()
        {
            var allocations =
                _allocator.Allocate(
                    15.01M,
                    new List<LineItem>
                    {
                        new LineItem("foo", 10, 1),
                        new LineItem("bar", 3, 2),
                        new LineItem("baz", 1, 0)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "bar", 2.81M, 2);
            AssertItem(allocations[1], "baz", 0, 0);
            AssertItem(allocations[2], "foo", 9.39M, 1);
        }

        [Fact]
        public void EverythingCompletelyDiscounted()
        {
            var allocations =
                _allocator.Allocate(
                    20,
                    new List<LineItem>
                    {
                        new LineItem("foo", 0, 1),
                        new LineItem("bar", 0, 2),
                        new LineItem("baz", 0, 34)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 0, 1);
            AssertItem(allocations[1], "bar", 0, 2);
            AssertItem(allocations[2], "baz", 0, 34);
        }

        [Fact]
        public void EverythingRemoved()
        {
            var allocations =
                _allocator.Allocate(
                    20,
                    new List<LineItem>
                    {
                        new LineItem("foo", 3, 0),
                        new LineItem("bar", 5, 0),
                        new LineItem("baz", 8, 0)
                    });

            Assert.Equal(3, allocations.Count);
            AssertItem(allocations[0], "foo", 0, 0);
            AssertItem(allocations[1], "bar", 0, 0);
            AssertItem(allocations[2], "baz", 0, 0);
        }

        private static void AssertItem(
            LineItem item,
            string expectedId,
            Money expectedPrice,
            int expectedQuantity)
        {
            Assert.Equal(expectedId, item.Id);
            Assert.Equal(expectedPrice, item.Price);
            Assert.Equal(expectedQuantity, item.Quantity);
        }
    }
}
