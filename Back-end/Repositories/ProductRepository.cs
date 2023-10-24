using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Product>();
        }

        public IEnumerable<Product> GetAll()
        {
            return _dbSet.ToList();
        }

        public Product GetById(int id)
        {
            return _dbSet.Find(id);
        }
    }
}