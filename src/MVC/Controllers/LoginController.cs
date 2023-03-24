using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVC.Models;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        async public Task<IActionResult> Form2(Models.LoginModel sm, PitchforkContext db)
        {

            ViewBag.Login = sm.Login;
            ViewBag.Password = sm.Password;

            // находим пользователя 
            User? user = db.Users.FirstOrDefault(p => p.Login == sm.Login && p.Password == sm.Password);

            if (user is null)
            {
                ViewData["Message"] = "User with such Login and Password does not exist";
            }
            else
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                        new Claim(ClaimTypes.Role, user.Role.Name.ToString())
                    };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect("/Home/");
            }
            return View("Form2");
        }

    }
}
