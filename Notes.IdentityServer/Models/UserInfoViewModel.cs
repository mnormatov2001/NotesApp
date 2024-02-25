using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Notes.IdentityServer.Models;

public class UserInfoViewModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }
}