using Application.Account;
using Core.Authentication;
using Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Account;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl ?? Url.Content("~/")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginInputDto input, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Redirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            LoginViewModel model = new LoginViewModel
            {
                Input = input,
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            return Redirect(returnUrl ?? Url.Content("~/"));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            RegisterViewModel model = new RegisterViewModel
            {
                ReturnUrl = returnUrl ?? Url.Content("~/")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterInputDto input, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Nickname = input.Nickname
                };

                await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, HCBRoles.Basic);

                    return RedirectToAction(nameof(RegisterComplete), routeValues: new { returnUrl });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            RegisterViewModel model = new RegisterViewModel
            {
                Input = input,
                ReturnUrl = returnUrl ?? Url.Content("~/")
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterComplete(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
    }
}
