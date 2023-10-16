using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface ICustomerService
    {
        CustomerDTO GetById(int id);
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> FilterAll(string searchfield);
        int Post(Customer customer);
        void Put(CustomerDocumentDTO customerDocumentDTO);
    }
}