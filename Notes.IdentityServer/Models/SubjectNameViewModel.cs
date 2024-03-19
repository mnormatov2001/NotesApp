using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models;

public class SubjectNameViewModel
{
    [Required]
    [MaxLength(128)]
    public string FirstName { get; set;}

    [Required]
    [MaxLength(128)]
    public string LastName { get; set;}
}