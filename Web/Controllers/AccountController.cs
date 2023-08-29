﻿using Application.Account;
using Core.Authorization;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Account;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<HCBUser> _signInManager;
        private readonly UserManager<HCBUser> _userManager;
        private readonly IUserStore<HCBUser> _userStore;
        private readonly IUserEmailStore<HCBUser> _emailStore;

        public AccountController(SignInManager<HCBUser> signInManager, UserManager<HCBUser> userManager, IUserStore<HCBUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<HCBUser>)_userStore;
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
                HCBUser user = new HCBUser
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
