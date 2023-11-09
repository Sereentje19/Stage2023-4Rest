using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Microsoft.EntityFrameworkCore;
using Stage4rest2023.Exceptions;

namespace Stage4rest2023.Repositories
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

        public (IEnumerable<object>, int) GetAllProducts(string searchfield, ProductType? dropdown, int page, int pageSize)
        {
            IQueryable<Product> query = _context.Products
                .Where(product => (string.IsNullOrEmpty(searchfield) ||
                                   product.SerialNumber.Contains(searchfield) ||
                                   product.ExpirationDate.ToString().Contains(searchfield) ||
                                   product.PurchaseDate.ToString().Contains(searchfield))
                                  && (dropdown == ProductType.Not_Selected || product.Type == dropdown));
            
            int numberOfProducts = query.Count();
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            
            var productList = query
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();
            
            return (productList, numberOfProducts);
        }

        public Product GetProductById(int id)
        {
            return _dbSet.Find(id);
        }

        public void AddProduct(Product product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.SerialNumber))
                {
                    throw new InputValidationException("Serie nummer is leeg.");
                }

                if (product.Type == ProductType.Not_Selected)
                {
                    throw new InputValidationException("Type is leeg.");
                }

                _dbSet.Add(product);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        public void PutProduct(Product product)
        {
            try
            {
                _dbSet.Update(product);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                List<LoanHistory> loans = _context.LoanHistory.Where(l => l.Product.ProductId == id).ToList();
                foreach (LoanHistory loan in loans)
                {
                    _context.LoanHistory.Remove(loan);
                }

                _dbSet.Remove(_dbSet.Find(id));
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }
    }
}