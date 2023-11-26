using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(".*[a-z].*")]
        public string NewPassword { get; set; }
    }
}
