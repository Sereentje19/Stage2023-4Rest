using DAL.Repositories;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves a paged list of products based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <param name="dropdown">The product type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns>
        /// A tuple containing a collection of products and pagination information.
        /// </returns>
        public async Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, ProductType? dropdown,
            int page, int pageSize)
        {
            (IEnumerable<object> products, int numberOfProducts) =
                await _productRepository.GetAllProducts(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfProducts, page, pageSize);
            return (products, pager);
        }

        /// <summary>
        /// Retrieves a list of product type strings from the underlying product repository.
        /// </summary>
        /// <returns>
        /// A list of strings representing product types.
        /// </returns>
        public List<string> GetProductTypeStrings()
        {
            return _productRepository.GetProductTypeStrings();
        }
        
        /// <summary>
        /// Retrieves a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="product">The Product object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PostProduct(Product product)
        {
            await _productRepository.AddProduct(product);
        }

        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="product">The Product object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task PutProduct(Product product)
        {
            await _productRepository.PutProduct(product);
        }

        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }
    }
}
