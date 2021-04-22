using ElectronicStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace ElectronicStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(LoginModel LoginModel)
        {
            EditAccountFoUser model = LoginModel.EditAccountFoUser;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View("Login");
            }
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            else
            {
                
                user.Email = model.Email.Trim();
                user.Name = model.Name.Trim();
                var _passwordValidator =
            HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var _passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                if (model.Password != null)
                {
                    IdentityResult resultPassword =
            await _passwordValidator.ValidateAsync(_userManager, user, model.Password.Trim());

                    if (resultPassword.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password.Trim());
                    }
                    else
                    {
                        foreach (var error in resultPassword.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("Login");
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            { 
                
                User user = new User { Date = DateTime.UtcNow, Email = model.Email.Trim(), UserName = model.Email.Trim(), Name = model.Name.Trim()};

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                EditAccountFoUser edit = new EditAccountFoUser() { Email = user.Email, Name = user.Name };
                return View(new LoginModel { LoginViewModel = new LoginViewModel { ReturnUrl = returnUrl }, EditAccountFoUser = edit });
            }
            return View(new LoginModel { LoginViewModel = new LoginViewModel { ReturnUrl = returnUrl } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel logmodel,string ReturnUrl)
        {
            LoginViewModel model = logmodel.LoginViewModel;

            if (ModelState.IsValid)
            {
               
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username and (or) password");
                }
            }
            return View(logmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
