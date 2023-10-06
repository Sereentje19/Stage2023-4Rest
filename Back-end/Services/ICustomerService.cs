using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> FilterAll(string searchfield);
        Customer GetById(int id);
        int Post(Customer customer);
        void Put(Customer customer);
    }
}