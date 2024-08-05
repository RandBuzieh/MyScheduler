using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Controllers
{
    public class GenerateScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
