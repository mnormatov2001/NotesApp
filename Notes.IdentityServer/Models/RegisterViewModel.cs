using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        [MaxLength(128)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
