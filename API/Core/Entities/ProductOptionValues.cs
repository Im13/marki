namespace Core.Entities
{
    public class ProductOptionValues : BaseEntity
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public int ValueId { get; set; }
        public string ValueName { get; set; }
        public ProductOptions ProductOption { get; set; }
    }
}