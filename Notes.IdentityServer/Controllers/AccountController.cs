using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.IdentityServer.Models;
using Notes.IdentityServer.Services;
using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Controllers
{
    [ApiController]
    [Produces("Application/json")]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, 
            EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> RequestConfirmationEmail(
            [EmailAddress] string email)
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new { error = $"'{email}' is not registered." });

            if (user.EmailConfirmed)
                return Conflict(new { error = $"'{email}' is already confirmed" });

            var success = await SendConfirmationEmail(user);
            if (!success)
                return Problem("failed to send email", 
                    statusCode: StatusCodes.Status500InternalServerError, 
                    title: "InternalServerError");

            return Ok();
        }

        private async Task<bool> SendConfirmationEmail(AppUser user)
        {
            var confirmationToken =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                new { email = user.Email, confirmationToken = confirmationToken },
                Request.Scheme)!;

            return await _emailService.SendAccountConfirmationEmailAsync(user.Email, callbackUrl);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(
            [EmailAddress] string email,
            [Required] string confirmationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Неверная ссылка подтверждения." });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { error = "Неверная ссылка подтверждения." });
            }

            if (user.EmailConfirmed)
                return Ok($"Почта '{email}' уже подтверждена!");

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                ModelState.AddModelError(string.Empty,
                    "Не удалось подтвердить почту.");
                return BadRequest(ModelState);
            }
            return Ok($"Почта '{email}' успешно подтверждена!");
        }

        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            var email = User.FindFirst("email")?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            var userInfo = new UserInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            };
            return Ok(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.FindFirst("email")?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
                return Ok();

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return Conflict(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSubjectName(SubjectNameViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.FindFirst("email")?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok();

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return Conflict(ModelState);
        }
    }
}
