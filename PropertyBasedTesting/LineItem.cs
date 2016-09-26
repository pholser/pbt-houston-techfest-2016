using System;

namespace PropertyBasedTesting
{
    public class LineItem
    {
        public LineItem(string id, Money price, int quantity)
        {
            if (price < 0)
                throw new ArgumentException("Negative price: " + price);
            if (quantity < 0)
                throw new ArgumentException("Negative quantity: " + quantity);

            Id = id;
            Price = price;
            Quantity = quantity;
        }

        public string Id { get; }
        public Money Price { get; }
        public int Quantity { get; }

        public Money Amount => Price * Quantity;

        public LineItem WithPrice(Money price)
        {
            return new LineItem(Id, price, Quantity);
        }

        public LineItem WithPriceAndQuantity(Money price, int quantity)
        {
            return new LineItem(Id, price, quantity);
        }
    }
}
