using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [StringLength(20, ErrorMessage = "*Длина имени должна быть между {2} и {1}.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [StringLength(20, ErrorMessage = "*Длина имени должна быть между {2} и {1}.", MinimumLength = 2)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "*Длина пароля должна быть между {2} и {1}.", MinimumLength = 5)]
        [RegularExpression(".*[a-z].*", 
            ErrorMessage = "*Пароль должен содержать хотя бы одну латинскую букву в нижнем регистре.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*Это поле является обязательным.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "*Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
