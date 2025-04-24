using GrӧkTube.DAO;
using GrӧkTube.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;


namespace GrӧkTube.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDAOImpl _userDAO;

        public HomeController(ILogger<HomeController> logger, UserDAOImpl userDAO)
        {
            _logger = logger;
            _userDAO = userDAO;
        }

        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                // Получаем данные из claims
                var username = User.Identity.Name;
                var userId = User.FindFirst("UserId")?.Value;

                var user = _userDAO.GetUserByLoginAndId(username, int.Parse(userId));
                
                ViewBag.User = new UserModel
                {
                    Username = username,
                    AvatarUrl = user.AvatarUrl

                };

                return View();
            }
           
                ViewBag.User = null;
                return View();
            }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
