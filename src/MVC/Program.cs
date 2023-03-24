using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using MVC.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Login";
                   options.AccessDeniedPath = "/accessdenied";
               });
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services); // calling ConfigureServices method

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();   // добавление middleware авторизации 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.MapGet("/accessdenied", async (HttpContext context) =>
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied");
            });


            app.Map("/admin", [Authorize(Roles = "Admin")] () => "Admin Panel");

   
            app.MapGet("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return "ƒанные удалены";
            });

            
            app.Run();
        }
    }
}