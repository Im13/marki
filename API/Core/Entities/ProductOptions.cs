namespace Core.Entities
{
    public class ProductOptions : BaseEntity
    {
        public string OptionName { get; set; }
        public List<ProductOptionValues> ProductOptionValues { get; set; }
    }
}