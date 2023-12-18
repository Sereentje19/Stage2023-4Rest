using DAL.Models;

namespace DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<object>, int)> GetAllEmployees(string searchfield, int page, int pageSize);

        Task<(IEnumerable<object>, int)> GetAllArchivedEmployees(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string searchfield);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeIsArchivedAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee entity);
        Task DeleteEmployeeAsync(int id);
    }
}
