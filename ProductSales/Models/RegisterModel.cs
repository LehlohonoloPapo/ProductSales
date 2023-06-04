using System.ComponentModel.DataAnnotations;

namespace ProductSales.Models
{
    public class RegisterModel
    {
        public Guid UserId { get; set; }

        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public Guid Role { get; set; }
    }
}
