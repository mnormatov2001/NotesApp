using System.ComponentModel.DataAnnotations;
using Notes.IdentityServer.Services.Interfaces;

namespace Notes.IdentityServer.Services;

public class EmailService
{
    private readonly IEmailSender _emailSender;

    public EmailService(IEmailSender emailSender, ILogger<EmailService> logger) =>
        _emailSender = emailSender;

    public async Task<bool> SendAccountConfirmationEmailAsync(
        [EmailAddress] string sendTo, [Url] string callbackUrl)
    {
        var subject = "Confirm your NoteXpress email address";
        var message = "Welcome to NoteXpress!\n" +
                      "Confirm your registration by following the link: " +
                      $"<a href=\"{callbackUrl}\">{callbackUrl}</a>";

        return await _emailSender.SendEmailAsync(sendTo, subject, message);
    }

    public async Task<bool> SendPasswordResetEmailAsync(
        [EmailAddress] string sendTo, [Url] string callbackUrl)
    {
        var subject = "Resetting NoteXpress account password";
        var message = "To reset your password, follow the link: " +
                      $"<a href=\"{callbackUrl}\">{callbackUrl}</a>";

        return await _emailSender.SendEmailAsync(sendTo, subject, message);
    }
}