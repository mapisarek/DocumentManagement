using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentManagement.DAL;
using DocumentManagement.Models;
using DocumentManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly LoginService _context;

        public AuthController(DMContext context)
        {
            _context = new LoginService(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var _user = _context.CheckCredential(user);
                if (_user == null)
                {
                    TempData["error"] = "Błędne dane logowania!";
                    return RedirectToAction("Index", "Auth");
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, _user[0].UserName.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, _user[0].UserRole));
                identity.AddClaim(new Claim(ClaimTypes.Email, _user[0].UserEmail));
                
                HttpContext.Session.SetString("UserEmail", _user[0].UserEmail);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (_user[0].UserRole == "Admin")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else if (_user[0].UserRole == "User")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Auth");
        }

        public IActionResult Forbidden()
        {
            TempData["error"] = "Permission denied!!";
            return RedirectToAction("Index", "Auth");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}