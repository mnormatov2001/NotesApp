using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Models
{
    public class SubjectNameViewModel
    {
        [Required]
        public string FirstName { get; set;}
        [Required]
        public string LastName { get; set;}
    }
}
