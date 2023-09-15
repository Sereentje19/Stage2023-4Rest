using System;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
        void Add(Customer entity);
        void Update(Customer entity);
        void Delete(Customer entity);
    }
}