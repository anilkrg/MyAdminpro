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

        public IActionResult Login(SignInModel model)
        {
            if(ModelState.IsValid)
            {
                var result = UserService.SignIn(model);
                if(result==SignInEnum.Sucess)
                {
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
    }
}
