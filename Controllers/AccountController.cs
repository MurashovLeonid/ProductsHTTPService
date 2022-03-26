using BrandsHTTPService.EntityModels.AuthentificationModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BrandsHTTPService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class AccountController : Controller
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }
        
        [HttpGet("Authorization")]
        public IActionResult Authorization()
        {
            return new JsonResult(new { mes = "Пожалуйста, переключите метод на POST, введите в тестировочной программе в поле URI после домена и порта введите следующие данные : {Account/Login} и в Body введите логин и пароль, спасибо!" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([Bind("UserEmail,UserPassword")] LoginModel model)
        {
            try {
                if (ModelState.IsValid)
                {
                    var user = await db.Users.Where(u => u.UserEmail == model.UserEmail && u.UserPassword == model.UserPassword).FirstAsync();
                    if (user != null)
                    {
                        await Authenticate(model.UserEmail); // аутентификация

                        return RedirectToAction("Index", "Account");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }              
            }
            catch(Exception ex)
            {
               
            }
            return new JsonResult(new { mes = "Неудачная авторизация" });
        }

        private async Task Authenticate(string userName)
        {
            try
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
           
           

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

