using EmployeeManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<Employee> Employees, int TotalCount)> GetEmployeesAsync(string? searchName, string? title, int page, int pageSize);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }

}
