using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace api_dating_app.Controllers
{
    /// <summary>
    /// Author: Petar Krastev
    /// </summary>
    public class FallbackController : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"),
                "text/HTML"
            );
        }
    }
}