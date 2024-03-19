using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models;

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
    [MinLength(5, ErrorMessage = "Minimum password length is 5.")]
    [MaxLength(128, ErrorMessage = "Maximum password length is 128.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [DisplayName("Confirm password")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }
}