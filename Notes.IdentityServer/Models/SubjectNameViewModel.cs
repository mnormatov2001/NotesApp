using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class SubjectNameViewModel
    {
        #nullable disable
        [Required]
        public string FirstName { get; set;}
        [Required]
        public string LastName { get; set;}
    }
}
