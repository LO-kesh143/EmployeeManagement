using EmployeeManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Web.Controllers
{
    public class TitleController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<TitleController> _logger;

        public TitleController(HttpClient client, ILogger<TitleController> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _client.GetFromJsonAsync<List<TitleSalaryViewModel>>("api/titleapi/list");
            return View(data);
        }
    }

}
