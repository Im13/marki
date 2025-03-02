namespace API.DTOs
{
    public class ShipToAddressDTO
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public int CityOrProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
    }
}