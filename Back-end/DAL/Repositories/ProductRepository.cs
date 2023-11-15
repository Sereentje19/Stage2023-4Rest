using PL.Exceptions;
using PL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Product>();
        }

        /// <summary>
        /// Retrieves a paged list of products based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for product serial numbers, expiration dates, or purchase dates.</param>
        /// <param name="dropdown">The product type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns>A tuple containing a collection of products and the total number of products.</returns>
        public async Task<(IEnumerable<object>, int)> GetAllProducts(string searchfield, ProductType? dropdown,
            int page, int pageSize)
        {
            IQueryable<Product> query = _context.Products
                .Where(product => (string.IsNullOrEmpty(searchfield) ||
                                   product.SerialNumber.Contains(searchfield) ||
                                   product.ExpirationDate.ToString().Contains(searchfield) ||
                                   product.PurchaseDate.ToString().Contains(searchfield))
                                  && (dropdown == ProductType.Not_Selected || product.Type == dropdown));

            int numberOfProducts = await query.CountAsync();
            int skipCount = Math.Max(0, (page - 1) * pageSize);

            var productList = await query
                .Skip(skipCount)
                .Take(pageSize)
                .Select(product => new ProductResponse
                {
                    ProductId = product.ProductId,
                    ExpirationDate = product.ExpirationDate,
                    PurchaseDate = product.PurchaseDate,
                    Type = product.Type.ToString(),
                    SerialNumber = product.SerialNumber
                })
                .ToListAsync();

            return (productList, numberOfProducts);
        }

        /// <summary>
        /// Retrieves a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<Product> GetProductById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="product">The Product object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.SerialNumber))
            {
                throw new InputValidationException("Serie nummer is leeg.");
            }

            if (product.Type == ProductType.Not_Selected)
            {
                throw new InputValidationException("Type is leeg.");
            }

            await _dbSet.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="product">The Product object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PutProduct(Product product)
        {
            _dbSet.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteProduct(int id)
        {
            _dbSet.Remove(_dbSet.Find(id));
            await _context.SaveChangesAsync();
        }
    }
}
