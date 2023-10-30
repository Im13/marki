namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string fullname, int cityOrProvinceId, int districtId, int wardId, string street, string phoneNumber)
        {
            Fullname = fullname;
            CityOrProvinceId = cityOrProvinceId;
            DistrictId = districtId;
            WardId = wardId;
            Street = street;
            PhoneNumber = phoneNumber;
        }

        public string Fullname { get; set; }
        public int CityOrProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
    }
}