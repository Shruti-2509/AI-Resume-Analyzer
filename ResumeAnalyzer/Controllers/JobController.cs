using Microsoft.AspNetCore.Mvc;

namespace ResumeAnalyzer.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
