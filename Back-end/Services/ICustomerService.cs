using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface ICustomerService
    {
        (IEnumerable<object>, Pager) GetAll(string searchfield, int page, int pageSize);
        IEnumerable<Customer> GetFilteredCustomers(string searchfield);
        Customer GetById(int id);
        int Post(Customer customer);
        void Put(Customer customers);
        void Delete(Customer customer);
    }
}