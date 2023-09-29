using System;
using Back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<Customer> _dbSet;

        public CustomerRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Customer>();
        }

        public Customer GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<Customer> FilterAll(string searchfield)
        {
            var filteredCustomers = _dbSet
            .Where(customer =>
                customer.Name.Contains(searchfield) ||
                customer.Email.Contains(searchfield) ||
                customer.CustomerId.ToString().Contains(searchfield)
            )
            .ToList();

            return filteredCustomers;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _dbSet.ToList();
        }

        public int Add(Customer entity)
        {
            if (entity.Name.Equals("") || entity.Email.Equals(""))
            {
                throw new Exception("Klant naam of email is leeg.");
            }

            List<Customer> customer = _dbSet.ToList();

            foreach (Customer c in customer)
            {
                if (c.Email == entity.Email && c.Name == entity.Name)
                {
                    return c.CustomerId;
                }
            }

            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity.CustomerId;
        }

    }
}