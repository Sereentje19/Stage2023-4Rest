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

        public IEnumerable<Product> GetAllProducts(string searchfield, ProductType? dropdown)
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

        public Product GetProductById(int id)
        {
            return _dbSet.Find(id);
        }

        public void PutProduct(Product product)
        {
            _dbSet.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            List<LoanHistory> loans = _context.LoanHistory.Where(l => l.Product.ProductId == id).ToList();
            foreach (var loan in loans)
            {
                _context.LoanHistory.Remove(loan);
            }

            _dbSet.Remove(_dbSet.Find(id));
            _context.SaveChanges();
        }
    }
}