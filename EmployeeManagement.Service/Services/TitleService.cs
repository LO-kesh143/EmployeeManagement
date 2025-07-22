using EmployeeManagement.Repository.DTOs;
using EmployeeManagement.Repository.Interfaces;
using EmployeeManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Services
{
    public class TitleService : ITitleService
    {
        private readonly ITitleRepository _repo;
        public TitleService(ITitleRepository repo) => _repo = repo;

        public async Task<List<TitleSalaryDto>> GetTitleSalaryStatsAsync()
            => await _repo.GetTitleSalaryStatsAsync();
    }
}
