namespace Core.Entities 
{
    public class ProductSKUValues : BaseEntity 
    {
        public int ValueTempId { get; set; }
        public ProductOptionValues ProductOptionValue { get; set; }
    }
}