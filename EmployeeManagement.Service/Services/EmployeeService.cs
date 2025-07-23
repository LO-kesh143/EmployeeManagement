using EmployeeManagement.Model.Models;
using EmployeeManagement.Repository.Interfaces;
using EmployeeManagement.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IGenericRepository<Employee> _genRepository;
        private readonly ITitleRepository _titleRepository;

        public EmployeeService(IGenericRepository<Employee> genRepository, ITitleRepository titleRepository)
        {
            _genRepository = genRepository;
            _titleRepository = titleRepository;
        }

        public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetEmployeesAsync(string? searchName, string? title, int page, int pageSize)
        {
            var queryable = _genRepository.Query().Include(e => e.Salaries).AsQueryable();

            // Business Logic: Filtering
            if (!string.IsNullOrWhiteSpace(searchName))
                queryable = queryable.Where(e => e.Name.Contains(searchName));

            if (!string.IsNullOrWhiteSpace(title))
                queryable = queryable.Where(e => e.Salaries.Any(s => s.Title.Contains(title)));

            var totalCount = await queryable.CountAsync();

            var employees = await queryable
                .OrderBy(e => e.EmployeeId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
        }

        //public async Task<Employee?> GetEmployeeByIdAsync(int id)
        //{
        //    return await _genRepository.GetByIdAsync(id);
        //}

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            // Business Logic: Prevent duplicate SSN
            var existing = await _genRepository.Query().AnyAsync(e => e.SSN == employee.SSN);
            if (existing)
                throw new Exception("Employee with the same SSN already exists.");

            return await _genRepository.AddAsync(employee);
        }

        public async Task<List<ViewTitleSalary>> GetTitleSalarySummaryAsync()
        {
            var salaries = await _titleRepository.GetTitleSalarySummaryAsync();

            var result = salaries
                .GroupBy(s => s.Title)
                .Select(g => new ViewTitleSalary
                {
                    Title = g.Key,
                    MinSalary = g.Min(x => x.MinSalary),
                    MaxSalary = g.Max(x => x.MaxSalary)
                })
                .ToList();

            return result;
        }

        //public async Task<Employee?> UpdateEmployeeAsync(Employee employee)
        //{
        //    // Business Logic: JoinDate validation
        //    if (employee.JoinDate > DateTime.Now)
        //        throw new Exception("Join date cannot be in the future.");

        //    return await _genRepository.UpdateAsync(employee);
        //}

        //public async Task<bool> DeleteEmployeeAsync(int id)
        //{
        //    return await _genRepository.DeleteAsync(id);
        //}

    }

}
