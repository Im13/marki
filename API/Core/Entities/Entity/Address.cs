using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Entity
{
    public class Address
    {
        public int Id { get; set; }
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
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}