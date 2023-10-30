using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AddressDTO
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public int CityOrProvinceId { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int WardId { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}