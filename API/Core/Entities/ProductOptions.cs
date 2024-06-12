namespace Core.Entities
{
    public class ProductOptions : BaseEntity
    {
        public string OptionName { get; set; }
        public ICollection<ProductOptionValues> ProductOptionValues { get; set; }
    }
}