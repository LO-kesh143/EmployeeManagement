using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeManagement.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IHttpClientFactory httpClientFactory, ILogger<EmployeeController> logger)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;
        }

        public async Task<IActionResult> EmployeeList(string? searchName, string? title, int page = 1, int pageSize = 10)
        {
            var url = $"api/employeeapi/list?searchName={searchName}&title={title}&page={page}&pageSize={pageSize}";
            var response = await _client.GetFromJsonAsync<EmployeeApiResponse>(url);

            if (response == null)
            {
                _logger.LogError("Failed to get employees from API.");
                return View(new List<EmployeeViewModel>());
            }

            ViewBag.TotalRecords = response.TotalRecords;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchName = searchName;
            ViewBag.Title = title;

            return View(response.Data); // assuming response.Data is already List<EmployeeViewModel>
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View(new EmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            // API call to add employee
            var response = await _client.PostAsJsonAsync("api/employeeapi/add", employee);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Employee added successfully.";
                return RedirectToAction("EmployeeList");
            }

            // Optionally, you can extract the error message from the response content
            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Failed to add employee. Server says: {errorContent}";

            return View(employee);
        }

        public async Task<IActionResult> TitleList()
        {
            var dtoList = await _client.GetFromJsonAsync<List<TitleSalaryViewModel>>("api/employeeapi/titlelist");

            var vmList = new List<TitleSalaryViewModel>();

            if (dtoList != null)
            {
                vmList = dtoList.Select(x => new TitleSalaryViewModel
                {
                    Title = x.Title,
                    MinSalary = x.MinSalary,
                    MaxSalary = x.MaxSalary
                }).ToList();
            }

            return View(vmList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> EditEmployee(int id)
        //{
        //    var employee = await _client.GetFromJsonAsync<EmployeeViewModel>($"api/employeeapi/{id}");
        //    if (employee == null)
        //    {
        //        TempData["Error"] = "Employee not found.";
        //        return RedirectToAction("EmployeeList");
        //    }
        //    return View(employee);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditEmployee(int id, EmployeeViewModel employee)
        //{
        //    if (id != employee.EmployeeId)
        //    {
        //        TempData["Error"] = "Invalid ID.";
        //        return View(employee);
        //    }

        //    var result = await _client.PutAsJsonAsync($"api/employeeapi/update/{id}", employee);
        //    if (result.IsSuccessStatusCode)
        //    {
        //        TempData["Success"] = "Employee updated successfully.";
        //        return RedirectToAction("EmployeeList");
        //    }

        //    TempData["Error"] = "Failed to update employee.";
        //    return View(employee);
        //}

        //public async Task<IActionResult> DeleteEmployee(int id)
        //{
        //    var result = await _client.DeleteAsync($"api/employeeapi/delete/{id}");
        //    TempData["Success"] = result.IsSuccessStatusCode ? "Employee deleted." : "Failed to delete employee.";
        //    return RedirectToAction("EmployeeList");
        //}
    }
}
