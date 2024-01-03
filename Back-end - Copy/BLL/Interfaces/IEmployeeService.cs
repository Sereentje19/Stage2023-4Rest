using DAL.Models;
using DAL.Models.Dtos.Requests;

namespace BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<object>, Pager)> GetPagedEmployees(string searchfield, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetPagedArchivedEmployees(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(string searchfield);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(EmployeeRequestDto employeeRequest);
        Task UpdateEmployeeIsArchivedAsync(EmployeeRequestDto employeeRequest);
        Task UpdateEmployeeAsync(EmployeeRequestDto employeesRequest);
        Task DeleteEmployeeAsync(int id);
    }
}
