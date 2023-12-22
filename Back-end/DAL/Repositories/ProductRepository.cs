using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
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
        /// Builds a query to retrieve products based on search criteria and dropdown selection.
        /// </summary>
        /// <param name="searchfield">The search criteria to filter products.</param>
        /// <param name="dropdown">The dropdown selection for filtering products by type.</param>
        /// <returns>An IQueryable<Product> representing the query to retrieve products.</returns>
        private IQueryable<Product> QueryGetProducts(string searchfield, string dropdown)
        {
            return _context.Products
                .Include(l => l.Type)
                .Where(product =>
                    (string.IsNullOrEmpty(searchfield) ||
                     product.SerialNumber.Contains(searchfield) ||
                     product.ExpirationDate.ToString().Contains(searchfield) ||
                     product.PurchaseDate.ToString().Contains(searchfield))
                    && (dropdown == "0" || product.Type.Id.ToString() == dropdown))
                .OrderBy(product => product.ExpirationDate);
        } 

        /// <summary>
        /// Retrieves a paged list of products based on search criteria, page, page size, filter, and dropdown selection.
        /// </summary>
        /// <param name="searchfield">The search criteria to filter products.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <param name="filter">The filter expression to further narrow down the products.</param>
        /// <param name="dropdown">The dropdown selection for filtering products by type.</param>
        /// <returns>
        /// A tuple containing an IEnumerable<object> representing the paged list of products and an integer representing
        /// the total number of products that match the filter criteria.
        /// </returns>
        private (IEnumerable<object>, int) GetPagedProductsInternal(string searchfield, int page, int pageSize,
            Expression<Func<Product, bool>> filter, string dropdown)
        {
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            IQueryable<Product> query = QueryGetProducts(searchfield, dropdown).Where(filter);
            int numberOfProducts = query.Count();

            IEnumerable<Product> productList = query
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (productList, numberOfProducts);
        }

        /// <summary>
        /// Retrieves all deleted products based on search criteria, page, page size, and dropdown selection.
        /// </summary>
        /// <param name="searchfield">The search criteria to filter deleted products.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The number of deleted products per page.</param>
        /// <param name="dropdown">The dropdown selection for filtering deleted products by type.</param>
        /// <returns>
        /// A Task containing a tuple with an IEnumerable<object> representing the paged list of deleted products
        /// and an integer representing the total number of deleted products that match the criteria.
        /// </returns>
        public Task<(IEnumerable<object>, int)> GetAllDeletedProducts(string searchfield, int page,
            int pageSize, string dropdown)
        {
            return Task.FromResult(GetPagedProductsInternal(searchfield, page, pageSize, item => item.IsDeleted,
                dropdown));
        }

        /// <summary>
        /// Retrieves all non-deleted products based on search criteria, page, page size, and dropdown selection.
        /// </summary>
        /// <param name="searchfield">The search criteria to filter non-deleted products.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The number of non-deleted products per page.</param>
        /// <param name="dropdown">The dropdown selection for filtering non-deleted products by type.</param>
        /// <returns>
        /// A Task containing a tuple with an IEnumerable<object> representing the paged list of non-deleted products
        /// and an integer representing the total number of non-deleted products that match the criteria.
        /// </returns>
        public Task<(IEnumerable<object>, int)> GetAllProducts(string searchfield, int page,
            int pageSize, string dropdown)
        {
            return Task.FromResult(GetPagedProductsInternal(searchfield, page, pageSize, item => !item.IsDeleted,
                dropdown));
        }

        /// <summary>
        /// Retrieves a list of product type strings from the enumeration of ProductType.
        /// </summary>
        /// <returns>
        /// A list of strings representing product types.
        /// </returns>
        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        /// <summary>
        /// Retrieves a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="product">The Product object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task CreateProductAsync(Product product)
        {
            if (product.Type.Name == "0")
            {
                throw new InputValidationException("Type is leeg.");
            }
            
            if (string.IsNullOrWhiteSpace(product.SerialNumber))
            {
                throw new InputValidationException("Serie nummer is leeg.");
            }

            ProductType type = await _context.ProductTypes
                .SingleOrDefaultAsync(t => t.Name == product.Type.Name);

            if (type != null)
            {
                product.Type = type;
            }

            await _dbSet.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="productRequest">The Product object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateProductAsync(ProductRequestDto productRequest)
        {
            Product existingProduct = await _dbSet
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.ProductId == productRequest.ProductId);

            if (existingProduct == null)
            {
                throw new NotFoundException("Geen product gevonden");
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(productRequest);
            existingProduct.Type = productRequest.Type;
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the deletion status of the specified product asynchronously.
        /// </summary>
        /// <param name="productRequest">The product request containing information about the product and the desired deletion status.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateIsDeletedAsync(ProductRequestDto productRequest)
        {
            Product existingProduct = await _dbSet
                .FirstOrDefaultAsync(p => p.ProductId == productRequest.ProductId);

            if (existingProduct == null)
            {
                throw new NotFoundException("Geen product gevonden");
            }

            existingProduct.TimeDeleted = productRequest.IsDeleted ? DateTime.Today : DateTime.MinValue;
            existingProduct.IsDeleted = productRequest.IsDeleted;
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteProductAsync(int id)
        {
            Product product = await _dbSet.FindAsync(id);

            if (product == null)
            {
                throw new NotFoundException("Geen product gevonden");
            }

            _dbSet.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}