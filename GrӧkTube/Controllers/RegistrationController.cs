using GrӧkTube.DAO;
using GrӧkTube.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using GrӧkTube.Entities;

namespace GrӧkTube.Controllers
{
    public class RegistrationController : Controller
    {   
        private readonly UserDAOImpl _userDAO = new UserDAOImpl();

        public async Task<IActionResult> Login(string login, string password)
        {
            if (IsValidUser(login, password))
            {
                var claims = new List<Claim>
        {
            new(ClaimTypes.Name, login)
           
        };

                var identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(2)
                    });

                return RedirectToAction("Stub", "Stub");
            }

            TempData["Error"] = "Invalid credentials";
            return BadRequest("");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); 
        }
        private bool IsValidUser(string login, string password)
        {
            var user = _userDAO.FindUser(login, password);

            if (user == null)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user) { 
            
            _userDAO.SaveUsers(user);
            await Login(user.Login,user.HashPassword);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            return View();
        }
    }
}
