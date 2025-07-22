using EmployeeManagement.API.DTOs;
using EmployeeManagement.Model.Models;
using EmployeeManagement.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeApiController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET api/employee/list?searchName=John&title=Manager&page=1&pageSize=10
        [HttpGet("list")]
        public async Task<IActionResult> GetEmployees(string? searchName, string? title, int page = 1, int pageSize = 10)
        {
            var (employees, totalCount) = await _employeeService.GetEmployeesAsync(searchName, title, page, pageSize);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(new
            {
                TotalRecords = totalCount,
                Data = employeeDtos
            });
        }

        // GET api/employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        // POST api/employee/add
        [HttpPost("add")]
        public async Task<IActionResult> AddEmployee(EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            var addedEmployee = await _employeeService.AddEmployeeAsync(employee);
            return Ok(_mapper.Map<EmployeeDto>(addedEmployee));
        }

        // PUT api/employee/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, EmployeeDto employeeDto)
        {
            if (id != employeeDto.EmployeeId)
                return BadRequest("Employee ID mismatch");

            var employee = _mapper.Map<Employee>(employeeDto);
            var updated = await _employeeService.UpdateEmployeeAsync(employee);
            if (updated == null) return NotFound();

            return Ok(_mapper.Map<EmployeeDto>(updated));
        }

        // DELETE api/employee/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            return deleted ? Ok(new { Message = "Employee deleted successfully." }) : NotFound();
        }
    }
}
