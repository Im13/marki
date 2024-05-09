namespace Core.Entities
{
    public class ProductOptions : BaseEntity
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public Product Product { get; set; }
    }
}