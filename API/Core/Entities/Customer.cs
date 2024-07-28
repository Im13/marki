namespace Core.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DOB { get; set; }
    }
}