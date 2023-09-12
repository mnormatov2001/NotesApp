using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Services.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync([EmailAddress] string emailAddress, string subject, string message);
    }
}
