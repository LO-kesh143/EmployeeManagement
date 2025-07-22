using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Model.Models
{
    public class EmployeeSalary
    {
        public int EmployeeSalaryId { get; set; }
        public int EmployeeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
