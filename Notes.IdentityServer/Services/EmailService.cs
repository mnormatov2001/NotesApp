using System.ComponentModel.DataAnnotations;
using Notes.IdentityServer.Services.Interfaces;

namespace Notes.IdentityServer.Services
{
    public class EmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender, ILogger<EmailService> logger) => 
            _emailSender = emailSender;

        public async Task<bool> SendAccountConfirmationEmailAsync(
            [EmailAddress] string sendTo, [Url] string callbackUrl)
        {
            var subject = "Подтверждение аккаунта NotesApp";
            var message = "Добро пожаловать в NotesApp!\n" +
                          "Подтвердите регистрацию, перейдя по ссылке: " +
                          $"<a href=\"{callbackUrl}\">{callbackUrl}</a>";

            return await _emailSender.SendEmailAsync(sendTo, subject, message);
        }

        public async Task<bool> SendPasswordResetEmailAsync(
            [EmailAddress] string sendTo, [Url] string callbackUrl)
        {
            var subject = "Сброс пароля аккаунта NotesApp";
            var message = "Для сброса пароля перейдите по ссылке: " +
                          $"<a href=\"{callbackUrl}\">{callbackUrl}</a>";

            return await _emailSender.SendEmailAsync(sendTo, subject, message);
        }
    }
}
