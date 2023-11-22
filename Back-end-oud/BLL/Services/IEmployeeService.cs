using Azure;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<object>, Pager)> GetPagedEmployee(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployee(string searchfield);
        Task<Employee> GetEmployeeById(int id);
        Task<int> PostEmployee(Employee employee);
        Task PutEmployee(Employee employees);
        Task DeleteEmployee(int id);
    }
}
