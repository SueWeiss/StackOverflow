using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace _5_01StackO.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SigningUp(User user, string password)
        {
            Manager mgr = new Manager(_connectionString);
            mgr.AddUser(user, password);
            return Redirect("/");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoggingIn(string email, string password)
        {
            Manager mgr = new Manager(_connectionString);
            var user = mgr.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid login attempt";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
                {
                    new Claim("user", email)
                };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }
    }
}