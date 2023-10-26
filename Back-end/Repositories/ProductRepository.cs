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

        public IEnumerable<Product> GetAll(string searchfield, ProductType? dropdown)
        {
            IQueryable<Product> query = from product in _context.Products
                                        where (string.IsNullOrEmpty(searchfield) ||
                                            product.SerialNumber.Contains(searchfield) ||
                                            product.ExpirationDate.ToString().Contains(searchfield) ||
                                            product.PurchaseDate.ToString().Contains(searchfield))
                                        && (dropdown == ProductType.Not_Selected || product.Type == dropdown)
                                        select product;

            var productList = query.ToList();
            return productList;
        }

        public Product GetById(int id)
        {
            return _dbSet.Find(id);
        }
    }
}