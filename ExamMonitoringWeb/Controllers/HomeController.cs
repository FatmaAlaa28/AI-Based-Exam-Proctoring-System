using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using ExamMonitoringWeb.Models;
using System.Threading.Tasks;

namespace ExamMonitoringWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}