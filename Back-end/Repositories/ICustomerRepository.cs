using System;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetById(int id);
        IEnumerable<Customer> FilterAll(string searchfield);
        IEnumerable<Customer> GetAll();
        int Add(Customer entity);
        void Update(Customer entity);
    }
}