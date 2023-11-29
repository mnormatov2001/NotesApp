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

        /// <summary>
        /// Requests account confirmation email for specified email address.
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthenticated</response>
        /// <response code="400">If the request is not validated or the specified email address is not registered</response>
        /// <response code="409">If the specified email address is already confirmed</response>
        /// <response code="500">If there was an error when sending email</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> RequestConfirmationEmail(
            [EmailAddress] string email)
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

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

        /// <summary>
        /// Confirms email for user's account.
        /// <para>
        /// This endpoint will be called when the confirmation link is followed.
        /// </para>
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="confirmationToken"> Confirmation token</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the request is not validated or the link is invalid</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(
            [EmailAddress] string email,
            [Required] string confirmationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new { error = "Неверная ссылка подтверждения." });

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

        /// <summary>
        /// Retrieves user information.
        /// </summary>
        /// <returns>Returns UserInfoViewModel</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthenticated</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(UserInfoViewModel), StatusCodes.Status200OK)]
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

        /// <summary>
        /// changes password for the user account.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the request is not validated</response>
        /// <response code="401">If the user is unauthenticated</response>
        /// <response code="409">If unable to change password</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            var email = User.FindFirst("email")?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
                return Ok();

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return Conflict(ModelState);
        }

        /// <summary>
        /// Changes user's first and last name.
        /// </summary>
        /// <param name="model">SubjectNameViewModel</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the request is not validated</response>
        /// <response code="401">If the user is unauthenticated</response>
        /// <response code="409">If unable to change user's first and last name</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> ChangeSubjectName(SubjectNameViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

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
