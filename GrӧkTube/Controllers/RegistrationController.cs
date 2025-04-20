using GrӧkTube.DAO;
using GrӧkTube.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using GrӧkTube.Entities;
using GrӧkTube.Service;
using GrӧkTube.Models.DTO;

namespace GrӧkTube.Controllers
{
    public class RegistrationController : Controller
    {   
        private readonly UserDAOImpl _userDAO = new UserDAOImpl();
        private readonly UserService _userService = new UserService();

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
           
            var user = _userDAO.GetUserByLogin(login);

          
            if (user == null || !_userService.IsPasswordCorrect(password, user))
            {
                return Json(new
                {
                    success = false,
                    error = "Неверный логин или пароль"
                });
            }

           
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, login),
        new Claim("UserId", user.Id.ToString())
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

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "Home")
            });
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
        public async Task<IActionResult> Form(RegistrationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            // Проверка уникальности логина
            if (_userDAO.LoginExists(dto.Login))
            {
                ModelState.AddModelError(nameof(dto.Login), "Логин уже занят");
                return View(dto);
            }

            var user = new User
            {
                Name = dto.Name,
                Login = dto.Login,
                HashPassword = _userService.GetHashPassword(dto.Password),
                Race = dto.Race
            };

            _userDAO.SaveUsers(user);
            await Login(user.Login,user.HashPassword);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Form()
        {
            return View();
        }
    }
}
