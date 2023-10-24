namespace Core.Entities
{
    public class Ward : BaseEntity
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string DivisionType { get; set; }
        public string ShortCodeName { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}