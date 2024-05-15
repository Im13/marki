namespace Core.Entities 
{
    public class ProductSKUValues : BaseEntity 
    {
        public int ProductId { get; set; }
        public int SKUId { get; set; }
        public int OptionId { get; set; }
        public int ValueId { get; set; }
        public ProductSKUs ProductSKU { get; set; }
        public ProductOptions ProductOption { get; set; }
        public ProductOptionValues ProductOptionValue { get; set; }
    }
}