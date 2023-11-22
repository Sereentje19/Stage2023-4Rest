using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace DAL.Repositories
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
