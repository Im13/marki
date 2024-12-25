namespace Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductIemOrdered itemOrdered, decimal price, int quantity, string optionValueCombination) 
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
            OptionValueCombination = optionValueCombination;
        }

        public ProductIemOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string OptionValueCombination { get; set; }
    }
}