using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "*Неверный пароль.", MinimumLength = 5)]
        [RegularExpression(".*[a-z].*",
            ErrorMessage = "*Неверный пароль.")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
