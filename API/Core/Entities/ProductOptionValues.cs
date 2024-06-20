namespace Core.Entities
{
    public class ProductOptionValues : BaseEntity
    {
        public string ValueName { get; set; }
        public int ValueTempId { get; set; }
        public ProductOptions ProductOption { get; set; }
    }
}