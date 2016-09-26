using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PropertyBasedTesting
{
    public class MoneyAllocator
    {
        public IList<LineItem> Allocate(Money total, IList<LineItem> targets)
        {
            if (targets.Count == 0 || total == 0)
                return targets;
            var signs = targets.Select(i => Math.Sign(i.Amount)).Distinct();
            if (signs.Count() > 1 && !signs.Contains(0))
                throw new ArgumentException("Mixed signs on line item amounts");
            int consensusSign = signs.FirstOrDefault(s => s != 0);
            if (consensusSign != Math.Sign(total) && consensusSign != 0)
                throw new ArgumentException("Mixed signs between total and line items");

            var targetSum =
                targets.Aggregate(Money.Zero, (acc, i) => acc + i.Amount);
            if (targetSum == total)
                return SameAllocations(targets);

            var zeroes = new List<LineItem>();
            var allocations = new List<LineItem>();
            var remaining = total;

            foreach (var lineItem in targets)
            {
                if (lineItem.Amount == 0)
                {
                    var zero = lineItem.WithPrice(0);
                    zeroes.Add(zero);
                    allocations.Add(zero);
                }
                else
                {
                    var ratio =
                        decimal.Divide(lineItem.Amount, targetSum).RoundDown(4);
                    Money allocatedPrice =
                        decimal.Divide(total * ratio, lineItem.Quantity).RoundDown(2);

                    var allocation = lineItem.WithPrice(allocatedPrice);
                    allocations.Add(allocation);
                    remaining -= allocation.Amount;

                    Contract.Assert(
                        Math.Sign(remaining) != -Math.Sign(total),
                        "Sign of remainder flipped: remaining = " + remaining
                            + ", total = " + total);
                }
            }

            if (targetSum != 0)
            {
                while (remaining != 0)
                    remaining = DistributeRemaining(remaining, allocations, zeroes);
            }

            return allocations;
        }

        private IList<LineItem> SameAllocations(IList<LineItem> originals)
        {
            return originals
                .Select(i => i.Amount == 0 ? i.WithPrice(0) : i)
                .ToList();
        }

        private Money DistributeRemaining(
            Money remaining,
            IList<LineItem> allocations,
            IList<LineItem> zeroes)
        {
            while (remaining != 0)
            {
                var item = allocations[0];
                allocations.RemoveAt(0);

                var penniesRemaining = (int) (remaining * 100);
                if (zeroes.Contains(item))
                {
                    allocations.Add(item);
                }
                else if (item.Quantity <= penniesRemaining)
                {
                    allocations.Add(item.WithPrice(item.Price + 0.01m));
                    remaining -= item.Quantity * 0.01M;
                }
                else
                {
                    allocations.Add(
                        item.WithPriceAndQuantity(
                            item.Price + 0.01m,
                            penniesRemaining));
                    allocations.Add(
                        item.WithPriceAndQuantity(
                            item.Price,
                            item.Quantity - penniesRemaining));
                    remaining = 0;
                }
            }

            return remaining;
        }
    }
}
