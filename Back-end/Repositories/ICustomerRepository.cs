using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetById(int id);
        List<Customer> GetAll();
        IEnumerable<Customer> FilterAll(string searchfield);
        int Add(Customer entity);
        void Update(Customer entity);
        void Delete(Customer entity);
    }
}