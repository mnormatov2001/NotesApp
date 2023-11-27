using Microsoft.AspNetCore.Identity;

namespace Notes.IdentityServer.Models
{
    public class AppUser : IdentityUser
    {
        #nullable disable
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
