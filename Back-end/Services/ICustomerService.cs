using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface ICustomerService
    {
        (IEnumerable<object>, Pager) GetPagedCustomers(string searchfield, int page, int pageSize);
        IEnumerable<Customer> GetFilteredCustomers(string searchfield);
        Customer GetCustomerById(int id);
        int PostCustomer(Customer customer);
        void PutCustomer(Customer customers);
        void DeleteCustomer(int id);
    }
}