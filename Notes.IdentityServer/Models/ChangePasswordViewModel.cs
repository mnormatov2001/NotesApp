using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models;

public class ChangePasswordViewModel
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(128)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}