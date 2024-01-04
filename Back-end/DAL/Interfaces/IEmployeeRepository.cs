using DAL.Models;
using DAL.Models.Dtos.Requests;

namespace DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<object>, int)> GetAllEmployees(string searchfield, int page, int pageSize);

        Task<(IEnumerable<object>, int)> GetAllArchivedEmployees(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string searchfield);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(Employee employeeRequest);
        Task UpdateEmployeeIsArchivedAsync(EmployeeRequestDto employeeRequest);
        Task UpdateEmployeeAsync(EmployeeRequestDto employeeRequest);
        Task DeleteEmployeeAsync(int id);
    }
}
