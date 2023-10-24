namespace Core.Entities
{
    public class Province : BaseEntity
    {
        public string Name { get; set; }
        public string DivisionType { get; set; }
        public int PhoneCode { get; set; }
        public string CodeName { get; set; }
    }
}