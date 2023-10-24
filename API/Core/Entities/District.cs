namespace Core.Entities
{
    public class District : BaseEntity
    {
        public string Name { get; set; }
        public string DivisionType { get; set; }
        public string CodeName { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}