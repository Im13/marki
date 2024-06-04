namespace Core.Entities
{
    public class ProductOptions : BaseEntity
    {
        public string OptionName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ICollection<ProductOptionValues> ProductOptionValues { get; set; }
    }
}