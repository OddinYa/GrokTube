using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrӧkTube.Controllers
{
    [Authorize]
    public class StubController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Error()
        {
            return View("Stub");
        }
    }
}
