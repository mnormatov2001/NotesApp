using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class ChangePasswordViewModel
    {
        #nullable disable
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(".*[a-z].*")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
