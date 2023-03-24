using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using Azure;
using Azure.Core;
using System;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var login = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
            var role = HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
            return View((login?.Value, role?.Value));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}