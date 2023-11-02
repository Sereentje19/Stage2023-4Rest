using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll(string searchfield);
        IEnumerable<Customer> GetFilteredCustomers(string searchfield);
        Customer GetCustomerById(int id);
        int AddCustomer(Customer entity);
        void UpdateCustomer(Customer entity);
        void DeleteCustomer(int id);
    }
}