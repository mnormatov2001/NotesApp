using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;
using Notes.IdentityServer.Services;

namespace Notes.IdentityServer.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly EmailService _emailService;

    public AuthController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IIdentityServerInteractionService interactionService,
        EmailService emailService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _interactionService = interactionService;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Login([Url] string returnUrl)
    {
        if (!ModelState.IsValid && !Url.IsLocalUrl(returnUrl))
            return BadRequest(ModelState);

        var loginVm = new LoginViewModel { ReturnUrl = returnUrl };
        var passwordResetQueryVm =
            new PasswordResetQueryViewModel { ReturnUrl = returnUrl };
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

        var callbackUrl = Url.Action("ConfirmEmail", "Account",
            new { email = user.Email, confirmationToken = confirmationToken },
            Request.Scheme)!;

        return await _emailService.SendAccountConfirmationEmailAsync(
            user.Email, callbackUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

        if (logoutRequest?.SignOutIFrameUrl != null &&
            logoutRequest.PostLogoutRedirectUri != null)
        {
            var url = string.Format("{0}?callbackUrl={1}",
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
            ModelState.AddModelError("Ошибка",
                "Неверная ссылка для сброса пароля.");
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            ModelState.AddModelError("Ошибка",
                "Неверная ссылка для сброса пароля.");
            return BadRequest(ModelState);
        }

        var okay = await _userManager.VerifyUserTokenAsync(user,
            _userManager.Options.Tokens.PasswordResetTokenProvider,
            UserManager<AppUser>.ResetPasswordTokenPurpose, passwordResetToken);
        if (!okay)
        {
            ModelState.AddModelError("Ошибка",
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
            ModelState.AddModelError("Ошибка",
                "Неверная ссылка для сброса пароля.");
            return BadRequest(ModelState);
        }

        var result = await _userManager.ResetPasswordAsync(user,
            model.PasswordResetToken, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            ModelState.AddModelError("Ошибка",
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
            Request.Scheme)!;

        return await _emailService.SendPasswordResetEmailAsync(user.Email, callbackUrl);
    }
}