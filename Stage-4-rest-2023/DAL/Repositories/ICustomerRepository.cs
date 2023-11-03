using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Repositories
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