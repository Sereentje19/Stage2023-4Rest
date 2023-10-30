using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll(string searchfield);
        IEnumerable<Customer> GetFilteredCustomers(string searchfield);
        Customer GetById(int id);
        int Add(Customer entity);
        void Update(Customer entity);
        void Delete(Customer customer);
    }
}