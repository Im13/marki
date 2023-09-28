namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string fullname, int cityOrProvinceId, int districtId, int wardId, string street)
        {
            Fullname = fullname;
            CityOrProvinceId = cityOrProvinceId;
            DistrictId = districtId;
            WardId = wardId;
            Street = street;
        }

        public string Fullname { get; set; }
        public int CityOrProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Street { get; set; }
    }
}