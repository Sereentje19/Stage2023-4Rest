using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
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