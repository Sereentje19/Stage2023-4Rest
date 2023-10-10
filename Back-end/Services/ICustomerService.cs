using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> FilterAll(string searchfield);
        CustomerDTO GetById(int id);
        int Post(Customer customer);
        void Put(Customer customer);
    }
}