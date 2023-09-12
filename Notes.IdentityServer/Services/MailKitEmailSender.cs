using Notes.IdentityServer.Services.Interfaces;

namespace Notes.IdentityServer.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        public Task<bool> SendEmailAsync(string emailAddress, string subject, string message)
        {
            return Task.FromResult(true);
        }
    }
}
