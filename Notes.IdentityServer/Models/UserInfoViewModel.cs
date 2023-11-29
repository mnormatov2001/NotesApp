using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class UserInfoViewModel
    {
        #nullable disable
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
