using Microsoft.AspNetCore.Mvc;
using MyAdminpro.Models.Repository.Intreface;
using MyAdminpro.Utils.Enums;
using MyAdminpro.ViewModel;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MyAdminpro.Controllers
{
    public class AccountController : Controller
    {
        private IUsers UserService;
        public AccountController(IUsers users)
        {
            UserService = users;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(SignInModel model)
        {
            if(ModelState.IsValid)
            {
                var result = UserService.SignIn(model);
                if(result==SignInEnum.Sucess)
                {

                    var claims = new List<Claim>() {
                   
                        new Claim(ClaimTypes.Name, model.Email),
                      
                };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    });
                    return RedirectToAction("Index", "Home");
                }
                else if(result==SignInEnum.WrongCredentials)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login credential");
                }
                else if(result==SignInEnum.NotVerified)
                {
                    ModelState.AddModelError(string.Empty, "IUser not verified please verify first");

                }
                else if(result==SignInEnum.Inactive)
                {
                    ModelState.AddModelError(string.Empty, "Your account is Incative");

                }
            }

            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
