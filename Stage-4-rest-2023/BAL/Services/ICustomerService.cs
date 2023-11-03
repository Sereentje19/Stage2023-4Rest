using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
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