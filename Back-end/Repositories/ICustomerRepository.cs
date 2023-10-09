using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface ICustomerRepository
    {
        CustomerDTO GetById(int id);
        IEnumerable<Customer> FilterAll(string searchfield);
        int Add(Customer entity);
        void Update(CustomerDTO entity);
    }
}