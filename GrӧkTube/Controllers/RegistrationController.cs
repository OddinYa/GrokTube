using GrӧkTube.DAO;
using GrӧkTube.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            new(ClaimTypes.Name, login),
            new("CustomClaim", "Example")
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

                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Invalid credentials";
            return RedirectToAction("Login");
        }

        private bool IsValidUser(string login, string password)
        {
            // Ваша проверка в базе данных
            return false;
        }
    }
}
