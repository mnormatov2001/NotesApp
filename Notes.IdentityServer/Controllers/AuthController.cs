using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.IdentityServer.Models;
using Notes.IdentityServer.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Notes.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEmailSender _emailSender;

        public AuthController(SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager, 
            IIdentityServerInteractionService interactionService, 
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login([Url] string returnUrl)
        {
            if (!ModelState.IsValid && !Url.IsLocalUrl(returnUrl))
                return BadRequest(ModelState);

            var loginVm = new LoginViewModel { ReturnUrl = returnUrl };
            var passwordResetQueryVm = new PasswordResetQueryViewModel { ReturnUrl = returnUrl };
            return View((loginVm, passwordResetQueryVm));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            var passwordResetQueryVm = new PasswordResetQueryViewModel 
                { Email = loginVm.Email, ReturnUrl = loginVm.ReturnUrl };

            if (!ModelState.IsValid)
            {
                ViewBag.LoginError = true;
                return View((loginVm, passwordResetQueryVm));
            }

            var user = await _userManager.FindByEmailAsync(loginVm.Email);
            if (user == null)
            {
                ViewBag.LoginError = true;
                ViewBag.EmailNotFoundError = true;
                return View((loginVm, passwordResetQueryVm));
            }

            var result = await _signInManager.PasswordSignInAsync(user, 
                loginVm.Password, false, false);
            if (!result.Succeeded)
            {
                ViewBag.LoginError = true;
                ViewBag.IncorrectPasswordError = true;
                return View((loginVm, passwordResetQueryVm));
            }

            return Redirect(loginVm.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register([Url] string returnUrl)
        {
            if (!ModelState.IsValid && !Url.IsLocalUrl(returnUrl))
                return BadRequest(ModelState);

            var model = new RegisterViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existUser = await _userManager.FindByEmailAsync(model.Email);
            if (existUser != null)
            {
                ViewBag.RegistrationError = true;
                ViewBag.BusyEmailError = true;
                return View(model);
            }

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = false,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                ViewBag.RegistrationError = true;
                return View(model);
            }

            var success = await SendConfirmationEmail(user);
            ViewBag.ConfirmationEmailSent = success;
            ViewBag.RegistrationSuccess = true;
            return View(model);
        }

        private async Task<bool> SendConfirmationEmail(AppUser user)
        {
            var confirmationToken = 
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Action("ConfirmEmail", "Auth",
                new { email = user.Email, confirmationToken = confirmationToken },
                Request.Scheme);

            var subject = "Подтверждение аккаунта NotesApp";
            var message = "Добро пожаловать в NotesApp!\n" +
                          "Подтвердите регистрацию, перейдя по ссылке: " +
                          $"<a href=\"{callbackUrl}\">{callbackUrl}</a>";

            return await _emailSender.SendEmailAsync(user.Email, subject, message);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (logoutRequest?.SignOutIFrameUrl != null &&
                logoutRequest.PostLogoutRedirectUri != null)
            {
                var url = string.Format("{0}?logout-callbackUrl={1}",
                    logoutRequest.PostLogoutRedirectUri,
                    logoutRequest.SignOutIFrameUrl);
                return Redirect(url);
            }

            if (logoutRequest?.PostLogoutRedirectUri != null)
                return Redirect(logoutRequest.PostLogoutRedirectUri);

            if (logoutRequest?.SignOutIFrameUrl != null)
                return Redirect(logoutRequest.SignOutIFrameUrl);

            return Ok("Выход выполнен успешно.");
        }

        [HttpGet]
        public async Task<IActionResult> RequestConfirmationEmail(
            [EmailAddress] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest($"\"{email}\" не зарегистрирован.");

            if (user.EmailConfirmed)
                return Ok($"Почта \"{email}\" уже подтверждена.");

            var success = await SendConfirmationEmail(user);
            if (!success)
                return Problem("Внутренняя ошибка сервера - " +
                               "не удалось отправить письмо подтверждения.");

            return Ok($"Письмо подтверждения отправлено на \"{email}\" .");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(
            [EmailAddress] string email, 
            [Required] string confirmationToken)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка подтверждения.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка подтверждения.");
                return BadRequest(ModelState);
            }

            if (user.EmailConfirmed)
                return Ok($"Почта \"{email}\" уже подтверждена!");

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) 
                    ModelState.AddModelError(error.Code, error.Description);
                
                ModelState.AddModelError(string.Empty, 
                    "Не удалось подтвердить почту.");
                return BadRequest(ModelState);
            }
            return Ok($"Почта \"{email}\" успешно подтверждена!");
        }

        [HttpPost]
        public async Task<IActionResult> RequestPasswordResetEmail(
            PasswordResetQueryViewModel model)
        {
            var loginVm = new LoginViewModel
            {
                Email = model.Email,
                ReturnUrl = model.ReturnUrl,
            };

            if (!ModelState.IsValid)
            {
                ViewBag.CanNotResetPasswordError = true;
                return View("Login", (loginVm, model));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.CanNotResetPasswordError = true;
                ViewBag.EmailNotFoundError = true;
                return View("Login", (loginVm, model));
            }

            if (!user.EmailConfirmed)
            {
                ViewBag.CanNotResetPasswordError = true;
                ViewBag.EmailNotConfirmedError = true;
                return View("Login", (loginVm, model));
            }

            var success = await SendPasswordResetEmail(user);
            if (!success)
            {
                ViewBag.CanNotResetPasswordError = true;
                ViewBag.SendingEmailError = true;
            }

            return View("Login", (loginVm, model));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(
            [EmailAddress] string email,
            [Required] string passwordResetToken)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка для сброса пароля.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка для сброса пароля.");
                return BadRequest(ModelState);
            }

            var okay = await _userManager.VerifyUserTokenAsync(user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                UserManager<AppUser>.ResetPasswordTokenPurpose, passwordResetToken);
            if (!okay)
            {
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка для сброса пароля.");
                return BadRequest(ModelState);
            }

            var model = new ResetPasswordViewModel
            {
                Email = email, 
                PasswordResetToken = passwordResetToken
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка для сброса пароля.");
                return BadRequest(ModelState);
            }

            var result = await _userManager.ResetPasswordAsync(user, 
                model.PasswordResetToken, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) 
                    ModelState.AddModelError(error.Code, error.Description);
                
                ModelState.AddModelError(string.Empty,
                    "Неверная ссылка для сброса пароля.");
                return BadRequest(ModelState);
            }

            return Ok("Новый пароль установлен.");
        }

        private async Task<bool> SendPasswordResetEmail(AppUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Auth",
                new { email = user.Email, passwordResetToken = token },
                Request.Scheme);

            var subject = "Сброс пароля аккаунта NotesApp";
            var message = "С возвращением в NotesApp!\n" +
                          "Для сброса пароля перейдите по ссылке: " +
                          $"<a href=\"{callbackUrl}\">{callbackUrl}</a>";

            return await _emailSender.SendEmailAsync(user.Email, subject, message);
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
                return Ok("Новый пароль установлен.");

            foreach (var error in result.Errors) 
                ModelState.AddModelError(error.Code, error.Description);

            return Conflict(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult>ChangeSubjectName(SubjectNameViewModel model)
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
                return Ok("Имя и фамилия сохранены.");

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return Conflict(ModelState);
        }
    }
}
