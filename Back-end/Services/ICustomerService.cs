using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetFilteredCustomers(string searchfield);
        CustomerDTO GetById(int id);
        int Post(Customer customer);
        void Put(CustomerDocumentDTO customerDocumentDTO);
    }
}