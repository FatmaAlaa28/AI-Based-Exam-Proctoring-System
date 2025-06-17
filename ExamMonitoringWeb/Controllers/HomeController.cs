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
            // Adjust the URL to match your app’s base URL
            //var response = await _httpClient.GetAsync("http://localhost:5000/api/detection/records");
            //if (response.IsSuccessStatusCode)
            //{
            //    var json = await response.Content.ReadAsStringAsync();
            //    var records = JsonSerializer.Deserialize<List<CheatingData>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            //    return View(records);
            //}
            return View();
        }
    }
}