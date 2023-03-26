using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using NLayerApp.BLL.DTO;
using NLayerApp.BLL.Interfaces;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // GET: Users
        [Authorize(Roles ="Admin,Moderator")]
        public IActionResult Index()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            return View(mapper.Map<IEnumerable<UserDTO>, List<User>>(_userService.GetUsers()));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }


        [Authorize(Roles = "Admin,Moderator")]
        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            var users = mapper.Map<IEnumerable<UserDTO>, List<User>>(_userService.GetUsers());
            if (id == null || users == null)
            {
                return NotFound();
            }

            var user = users.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_roleService.GetRoles(), "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Surname,DateOfBirth,Login,Password")] User user)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            var users = mapper.Map<IEnumerable<UserDTO>, List<User>>(_userService.GetUsers());
            var mapper2 = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            ViewData["Message"] = "";
            user.RoleId = 3;
            user.IsDeleted = false;
            if (!users.Any(p => p.Login == user.Login))
            {
                if (ModelState.IsValid)
                {
                    _userService.CreateUser(mapper2.Map<User, UserDTO>(user));
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var query = from state in ModelState.Values
                                from error in state.Errors
                                select error.ErrorMessage;

                    var errorList = query.ToList();
                    ViewData["Message"] = string.Join("; ", errorList);
                }
            }
            else
            {
                ViewData["Message"] = "User with this Login already exists";
            }
            ViewData["RoleId"] = new SelectList(_roleService.GetRoles(), "Id", "Id", user.RoleId);
            return View(user);
        }
    }
}
