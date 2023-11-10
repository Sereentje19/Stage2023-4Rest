using LeanSharp;
using Stage4rest2023.Exceptions;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Stage4rest2023.Models.Responses;

namespace Stage4rest2023.Repositories
{
    public interface ICustomerRepository
    {
        Task<(IEnumerable<object>, int)> GetAllCustomers(string searchfield, int page, int pageSize);
        Task<IEnumerable<Customer>> GetFilteredCustomers(string searchfield);
        Task<Customer> GetCustomerById(int id);
        Task<int> AddCustomer(Customer customer);
        Task UpdateCustomer(Customer entity);
        Task DeleteCustomer(int id);
    }
}