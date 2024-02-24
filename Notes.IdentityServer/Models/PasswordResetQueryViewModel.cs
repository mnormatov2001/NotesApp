using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models
{
    public class PasswordResetQueryViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }
    }
}