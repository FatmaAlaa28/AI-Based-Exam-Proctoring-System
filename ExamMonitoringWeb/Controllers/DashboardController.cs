using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamMonitoringWeb.Controllers
{
    
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
