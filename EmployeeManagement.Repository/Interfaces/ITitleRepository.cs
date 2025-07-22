using EmployeeManagement.Model.Models;
using EmployeeManagement.Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Repository.Interfaces
{
    public interface ITitleRepository
    {
        Task<List<TitleSalaryDto>> GetTitleSalaryStatsAsync();
    }
}
