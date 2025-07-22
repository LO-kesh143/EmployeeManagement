using EmployeeManagement.API.DTOs;
using EmployeeManagement.Model.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace EmployeeManagement.API.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<EmployeeSalary, EmployeeSalaryDto>().ReverseMap();
        }
    }
}
