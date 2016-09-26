using System;
using static System.Decimal;

namespace PropertyBasedTesting
{
    public sealed class Money
    {
        public static readonly Money Zero = new Money(0);

        private readonly decimal _value;

        public Money(decimal value)
        {
            _value = Round(value, 2, MidpointRounding.ToEven);
        }

        public static implicit operator Money(decimal value)
        {
            return new Money(value);            
        }

        public static implicit operator decimal(Money m)
        {
            return m._value;
        }

        public static Money operator +(Money first, Money second)
        {
            return new Money(first._value + second._value);
        }

        public static Money operator -(Money first, Money second)
        {
            return new Money(first._value - second._value);
        }

        public static Money operator *(Money original, int multiplier)
        {
            return new Money(original._value * multiplier);
        }

        public static bool operator ==(Money first, Money second)
        {
            return Equals(first, second);
        }

        public static bool operator !=(Money first, Money second)
        {
            return !(first == second);
        }

        public static bool operator <(Money first, Money second)
        {
            return first._value < second._value;
        }

        public static bool operator >(Money first, Money second)
        {
            return first._value > second._value;
        }

        public static bool operator <=(Money first, Money second)
        {
            return first._value <= second._value;
        }

        public static bool operator >=(Money first, Money second)
        {
            return first._value >= second._value;
        }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(o, this)) return true;

            var other = o as Money;
            return _value == other?._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{_value:C}";
        }
    }
}
