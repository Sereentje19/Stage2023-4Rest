using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface ICustomerService
    {
        Task<(IEnumerable<object>, Pager)> GetPagedCustomers(string searchfield, int page, int pageSize);
        Task<IEnumerable<Customer>> GetFilteredCustomers(string searchfield);
        Task<Customer> GetCustomerById(int id);
        Task<int> PostCustomer(Customer customer);
        Task PutCustomer(Customer customers);
        Task DeleteCustomer(int id);
    }
}