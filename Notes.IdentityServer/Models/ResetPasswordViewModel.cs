using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required] 
        public string PasswordResetToken { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        [MaxLength(128)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
