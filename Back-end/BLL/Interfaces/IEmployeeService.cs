using DAL.Models;

namespace BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<object>, Pager)> GetPagedEmployees(string searchfield, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetPagedArchivedEmployees(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string searchfield);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeIsArchivedAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employees);
        Task DeleteEmployeeAsync(int id);
    }
}
