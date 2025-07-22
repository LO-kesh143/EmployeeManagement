using AutoMapper;
using EmployeeManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(HttpClient client, ILogger<EmployeeController> logger)
        {
            _client = client;
            _logger = logger;
        }

        public class EmployeeApiResponse
        {
            public int TotalRecords { get; set; }
            public List<EmployeeViewModel> Data { get; set; } = new();
        }

        public async Task<IActionResult> Index(string? searchName, string? title, int page = 1, int pageSize = 10)
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

            return View(response.Data);
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel employee)
        {
            var response = await _client.PostAsJsonAsync("api/employeeapi/add", employee);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Employee added successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to add employee.";
            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _client.GetFromJsonAsync<EmployeeViewModel>($"api/employeeapi/{id}");
            if (employee == null)
            {
                TempData["Error"] = "Employee not found.";
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel employee)
        {
            if (id != employee.EmployeeId)
            {
                TempData["Error"] = "Invalid ID.";
                return View(employee);
            }

            var result = await _client.PutAsJsonAsync($"api/employeeapi/update/{id}", employee);
            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Employee updated successfully.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to update employee.";
            return View(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _client.DeleteAsync($"api/employeeapi/delete/{id}");
            TempData["Success"] = result.IsSuccessStatusCode ? "Employee deleted." : "Failed to delete employee.";
            return RedirectToAction("Index");
        }
    }



}
