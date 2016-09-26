using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace PropertyBasedTesting.Tests
{
    public class MoneyAllocatorProperties
    {
        public static Arbitrary<LineItem> AnyLineItem
        {
            get
            {
                var lineItem =
                    from id in Arb.Generate<string>()
                    from price in Arb.Generate<decimal>()
                        .Where(p => p > 0 && p < 100)
                    from quantity in Arb.Generate<PositiveInt>()
                        .Where(q => q.Get < 8)
                    select new LineItem(
                        id,
                        price,
                        quantity.Get);
                return lineItem.ToArbitrary();
            }
        }

        public static Arbitrary<IList<LineItem>> ListsOfLineItems
        {
            get
            {
                var sequence = AnyLineItem.Generator.ListOf();
                return sequence.ToArbitrary();
            }
        }

        public static Arbitrary<Money> LumpSumToDistribute
        {
            get;
        } = Arb.Generate<decimal>()
                .Where(a => a >= 100 && a <= 10000)
                .Select(a => new Money(a))
                .ToArbitrary();

        [Property]
        public Property AtLeastAsManyAllocations()
        {
            return Prop.ForAll(
                LumpSumToDistribute,
                ListsOfLineItems,
                (amount, items) =>
                {
                    var allocations =
                        new MoneyAllocator().Allocate(
                            amount,
                            items);
                    Assert.True(
                        allocations.Count >= items.Count);
                });
        }

        [Property]
        public Property AllocatesOnlyAsMuchAsGiven()
        {
            return Prop.ForAll(
                LumpSumToDistribute,
                ListsOfLineItems,
                (amount, items) =>
                {
                    var allocations =
                        new MoneyAllocator().Allocate(
                            amount,
                            items);
                    Assert.Equal(
                        amount,
                        allocations.Aggregate(
                            Money.Zero,
                            (acc, i) => acc + i.Amount));
                });
        }
    }
}
