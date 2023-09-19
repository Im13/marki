using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*\\d).{4,8}$", ErrorMessage = "Password must be between 4 and 8 digits long and include at least one numeric digit.")]
        public string Password { get; set; }
    }
}