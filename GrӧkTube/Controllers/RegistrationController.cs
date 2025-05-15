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
using System.Reflection;
using static System.Net.WebRequestMethods;
using GrӧkTube.Repository;

namespace GrӧkTube.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserDAOImpl _userDAO;
        private readonly UserService _userService;


        public RegistrationController(UserDAOImpl userDAO, UserService userService)
        {
            _userDAO = userDAO;
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            
            var user = _userDAO.FindUser(login, password);

          
            if (user == null)
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
                  "CookieAuth",
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
            await HttpContext.SignOutAsync("CookieAuth");
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
            if (ModelState.IsValid)
            {
                // Проверка капчи
                if (dto.CaptchaDrawnDigit != dto.CaptchaExpectedDigit) 
                {
                    ModelState.AddModelError("CaptchaDrawnDigit", "Пожалуйста, правильно нарисуйте цифру");
                    dto.CaptchaExpectedDigit = new Random().Next(0, 10).ToString();

                    ViewBag.User = null;
                    return View("Form", dto);
                }
            }
               

                var user = new User
                {
                    Name = dto.Name,
                    Login = dto.Login,
                    PhoneNumber = dto.PhoneNumber,
                    HashPassword = _userService.GetHashPassword(dto.Password),
                    Race = dto.Race,
                    AvatarUrl = string.IsNullOrEmpty(dto.AvatarUrl) ? null : dto.AvatarUrl

                };

                _userDAO.SaveUsers(user);


            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, user.Login),
                 new Claim("UserId", user.Id.ToString())
             };

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                 "CookieAuth", 
                 new ClaimsPrincipal(identity),
                 new AuthenticationProperties
                 {
                     IsPersistent = true,
                     ExpiresUtc = DateTime.UtcNow.AddHours(2)
                 }
                 );


            return RedirectToAction("Index", "Home");
            }




        [HttpGet]
        public async Task<IActionResult> Form()
        {
            RegistrationDto model = new RegistrationDto
            {
                CaptchaExpectedDigit = new Random().Next(0, 10).ToString()
            };

            ViewBag.User = null;
            return View("Form",model);
        }


        [HttpGet]
        public async Task<IActionResult> FormPro()
        {
            string urlAva = "https://masterpiecer-images.s3.yandex.net/e8c1a89874a011eea380da477c0f1ee2:upscaled";

            RegistrationDto dto = new RegistrationDto()
            {
                AvatarUrl = urlAva
            };

            ViewBag.User = null;
            return View("FormPro",dto);
        }

    }
}
