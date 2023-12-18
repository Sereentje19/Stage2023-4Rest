using DAL.Models;

namespace BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<object>, Pager)> GetPagedEmployee(string searchfield, int page, int pageSize);
        Task<(IEnumerable<object>, Pager)> GetPagedArchivedEmployee(string searchfield, int page, int pageSize);
        Task<IEnumerable<Employee>> GetFilteredEmployee(string searchfield);
        Task<Employee> GetEmployeeById(int id);
        Task<int> PostEmployee(Employee employee);
        Task PutEmployeeIsArchived(Employee employee);
        Task PutEmployee(Employee employees);
        Task DeleteEmployee(int id);
    }
}
