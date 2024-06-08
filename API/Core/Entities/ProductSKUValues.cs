namespace Core.Entities 
{
    public class ProductSKUValues : BaseEntity 
    {
        public int OptionId { get; set; }
        public int ProductSKUId { get; set; }
        public ProductSKUs ProductSKU { get; set; }
        public ProductOptions ProductOption { get; set; }
        public ProductOptionValues ProductOptionValue { get; set; }
    }
}