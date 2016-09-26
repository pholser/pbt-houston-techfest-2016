using System.Linq;
using System.Numerics;
using FsCheck;
using FsCheck.Xunit;

namespace PropertyBasedTesting.Tests
{
    public class PrimeFactorsProperties
    {
        private static Arbitrary<int> PossiblePrimes()
        {
           return Arb.Generate<int>()
                .Where(n => n > 1)
                .ToArbitrary();
        }

        [Property]
        public Property FactorsMultiplyToOriginal()
        {
            return Prop.ForAll(
                PossiblePrimes(),
                n =>
                {
                    var product =
                        PrimeFactors.Of(n)
                            .Aggregate((acc, val) => acc * val);
                    return product == n;
                });
        }

        [Property]
        public Property FactorsArePrime()
        {
            return Prop.ForAll(
                PossiblePrimes(),
                n => PrimeFactors.Of(n)
                    .All(f => new BigInteger(f).IsProbablePrime(100)));
        }

        [Property]
        public Property FactorsAreUnique()
        {
            return Prop.ForAll(
                PossiblePrimes(),
                PossiblePrimes(),
                (m, n) => PrimeFactors.Of(m) != PrimeFactors.Of(n));
        }
    }
}
