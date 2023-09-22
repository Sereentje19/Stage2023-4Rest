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

        public IEnumerable<Customer> GetAll()
        {
            return _dbSet.ToList();
        }

        public int Add(Customer entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity.CustomerId;
        }

    }
}