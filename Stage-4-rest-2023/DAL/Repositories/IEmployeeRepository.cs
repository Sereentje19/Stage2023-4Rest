using LeanSharp;
using Stage4rest2023.Exceptions;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Stage4rest2023.Models.Responses;

namespace Stage4rest2023.Repositories
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<object>, int)> GetAllEmployee(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployee(string searchfield);
        Task<Employee> GetEmployeeById(int id);
        Task<int> AddEmployee(Employee employee);
        Task UpdateEmployee(Employee entity);
        Task DeleteEmployee(int id);
    }
}