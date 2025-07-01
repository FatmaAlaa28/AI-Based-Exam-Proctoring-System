using System;
using System.Threading.Tasks;
using ExamMonitoringWeb.DTO;
using ExamMonitoringWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamMonitoringWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Role = model.Role,
                    RegistrationDate = DateTime.Now,
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogIn user)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _userManager.FindByEmailAsync(user.Email);
                if (checkUser != null && checkUser.Role != Role.Student
                    && (await _userManager.CheckPasswordAsync(checkUser, user.Password)))
                {
					HttpContext.Session.SetString("UserName", checkUser.UserName);
                    HttpContext.Session.SetString("Role", checkUser.Role.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No user found with this email.");
                }
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            // Sign out from ASP.NET Identity
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogIn", "Account");
        }
    }
}