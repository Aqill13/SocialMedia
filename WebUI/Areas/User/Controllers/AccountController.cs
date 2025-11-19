using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.User.Models;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        // Register
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                int controleCode = new Random().Next(100000, 999999);
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ControleCode = controleCode
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    await _emailService.SendEmailAsync($"{model.FirstName} {model.LastName}", user.Email, "Account Verification",
                        $"<h3>Your verification code is: {controleCode}</h3><p>Please enter this code to verify your account.</p>");
                    return RedirectToAction(nameof(VerifyAccount), new
                    {
                        email = user.Email,
                        name = user.FirstName + " " + user.LastName
                    });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        // Verify Account
        [HttpGet]
        public IActionResult VerifyAccount(string email, string name)
        {
            var model = new VerifyAccountViewModel
            {
                Email = email,
                FullName = name
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAccount(VerifyAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }
            if (user.ControleCode != model.ControleCode)
            {
                ModelState.AddModelError("", "Invalid verification code");
                return View(model);
            }
            user.ControleCode = 0;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // Resend Verification Code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerificationCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction(nameof(Register));
            int newControleCode = new Random().Next(100000, 999999);
            user.ControleCode = newControleCode;
            await _userManager.UpdateAsync(user);
            await _emailService.SendEmailAsync($"{user.FirstName} {user.LastName}", user.Email!, "Resend Account Verification",
                $"<h3>Your new verification code is: {newControleCode}</h3><p>Please enter this code to verify your account.</p>");
            return RedirectToAction(nameof(VerifyAccount), new
            {
                email = user.Email,
                name = user.FirstName + " " + user.LastName
            });
        }

        // Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !user.EmailConfirmed)
            {
                ModelState.AddModelError("", "This email has not been verified in our system");
                return View(model);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme
            );
            await _emailService.SendEmailAsync(
                $"{user.FirstName} {user.LastName}",
                model.Email,
                "Password reset",
                $@"
                    <h3>Hi, {user.FirstName}</h3>
                    <p>You requested to reset your password.</p>
                    <p>Click the link below to set a new password:</p>
                    <p><a href=""{resetLink}"">Reset Password</a></p>
                    <p>If you did not request this, please ignore this email.</p>
                "
            );
            ViewBag.Message = "If this email exists in our system, a reset link has been sent.";
            return View();
        }

        // Reset password
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction(nameof(Login));
            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return RedirectToAction(nameof(Login));
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
                return RedirectToAction(nameof(Login));
            return View(model);
        }

        // Login
        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            if (!user.EmailConfirmed)
            {
                return RedirectToAction(nameof(VerifyAccount), new
                {
                    email = user.Email,
                    fullName = user.FirstName + " " + user.LastName
                });
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home", new { area = "" });
            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                if (lockoutEnd.HasValue)
                {
                    var remaining = lockoutEnd.Value - DateTimeOffset.UtcNow;
                    if (remaining.TotalSeconds > 0)
                    {
                        var minutes = remaining.Minutes.ToString("00");
                        var seconds = remaining.Seconds.ToString("00");
                        ModelState.AddModelError("", $"Your account is locked. Time left: {minutes}:{seconds}");
                    }
                    else
                        ModelState.AddModelError("", "Your account is locked. Please try again later.");
                }
                return View(model);
            }
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
