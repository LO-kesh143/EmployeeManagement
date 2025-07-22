using EmployeeManagement.Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Interfaces
{
    public interface ITitleService
    {
        Task<List<TitleSalaryDto>> GetTitleSalaryStatsAsync();
    }
}
