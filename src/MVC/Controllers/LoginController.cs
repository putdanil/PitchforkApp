using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using NLayerApp.BLL.DTO;
using NLayerApp.BLL.Interfaces;
using System;
using System.Diagnostics;
using System.Security.Claims;


namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        async public Task<IActionResult> Form2(LoginModel sm)
        {

            ViewBag.Login = sm.Login;
            ViewBag.Password = sm.Password;

            // находим пользователя 
            var (login, role) = _userService.FindUserByLogin(sm.Login);

            if (login is null)
            {
                ViewData["Message"] = "User with such Login and Password does not exist";
            }
            else
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                        new Claim(ClaimTypes.Role, role)
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
