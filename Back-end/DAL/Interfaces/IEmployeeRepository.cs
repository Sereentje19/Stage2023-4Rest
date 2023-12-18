using DAL.Models;

namespace DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<object>, int)> GetAllEmployee(string searchfield, int page, int pageSize);

        Task<(IEnumerable<object>, int)> GetAllArchivedEmployees(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployee(string searchfield);
        Task<Employee> GetEmployeeById(int id);
        Task<int> AddEmployee(Employee employee);
        Task UpdateEmployeeIsArchived(Employee employee);
        Task UpdateEmployee(Employee entity);
        Task DeleteEmployee(int id);
    }
}
