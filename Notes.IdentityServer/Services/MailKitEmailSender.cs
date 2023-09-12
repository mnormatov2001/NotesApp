using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Notes.IdentityServer.Services.Interfaces;

namespace Notes.IdentityServer.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailKitEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string emailAddress, string subject, string message)
        {
            var senderUserName = _configuration.GetSection("SmtpClient")["UserName"];
            var senderPassword = _configuration.GetSection("SmtpClient")["Password"];
            var host = _configuration.GetSection("SmtpClient")["Host"];
            var port = _configuration.GetSection("SmtpClient").GetValue<int>("Port");
            var useSsl = _configuration.GetSection("SmtpClient").GetValue<bool>("UseSsl");

            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Notes.App", senderUserName));
            emailMessage.To.Add(new MailboxAddress("", emailAddress));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(host, port, useSsl);
                await client.AuthenticateAsync(senderUserName, senderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                // log
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
    }
}
