using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

#nullable disable
namespace Notes.IdentityServer.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(128)]
        public string FirstName { get; set; }

        [MaxLength(128)]
        public string LastName { get; set; }
    }
}
