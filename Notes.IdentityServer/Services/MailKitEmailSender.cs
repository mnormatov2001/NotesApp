using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Notes.IdentityServer.Services.Interfaces;

namespace Notes.IdentityServer.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailKitEmailSender> _logger;

        public MailKitEmailSender(IConfiguration configuration, ILogger<MailKitEmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string emailAddress, string subject, string message)
        {
            var senderUsername = _configuration["SmtpClient:Username"];
            var senderPassword = _configuration["SmtpClient:Password"];
            var host = _configuration["SmtpClient:Host"];
            var port = _configuration.GetRequiredSection("SmtpClient").GetValue<int>("Port");
            var useSsl = _configuration.GetRequiredSection("SmtpClient").GetValue<bool>("UseSsl");

            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Notes.App", senderUsername));
            emailMessage.To.Add(new MailboxAddress("", emailAddress));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(host, port, useSsl);
                await client.AuthenticateAsync(senderUsername, senderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to send email.");
                return false;
            }

            return true;
        }
    }
}
