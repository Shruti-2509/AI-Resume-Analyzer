using Microsoft.AspNetCore.Mvc;
using ResumeAnalyzer.Models;
using ResumeAnalyzer.Services;
using System.Diagnostics;

namespace ResumeAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // ✅ ADD THIS METHOD HERE
        [HttpPost]
        public async Task<IActionResult> Chat(string message)
        {
            var reply = await ChatbotService.GetResponse(message);
            return Json(reply);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}