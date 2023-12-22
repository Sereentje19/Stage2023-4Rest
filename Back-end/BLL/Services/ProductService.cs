using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;

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
        public async Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, string dropdown,
            int page, int pageSize)
        {
            (IEnumerable<object> products, int numberOfProducts) =
                await _productRepository.GetAllProducts(searchfield, page, pageSize, dropdown);
            Pager pager = new Pager(numberOfProducts, page, pageSize);
            return (products, pager);
        }
        
        public async Task<(IEnumerable<object>, Pager)> GetAllDeletedProducts(string searchfield, string dropdown,
            int page, int pageSize)
        {
            (IEnumerable<object> products, int numberOfProducts) =
                await _productRepository.GetAllDeletedProducts(searchfield, page, pageSize, dropdown);
            Pager pager = new Pager(numberOfProducts, page, pageSize);
            return (products, pager);
        }

        /// <summary>
        /// Retrieves a list of product type strings from the underlying product repository.
        /// </summary>
        /// <returns>
        /// A list of strings representing product types.
        /// </returns>
        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return await _productRepository.GetProductTypesAsync();
        }
        
        /// <summary>
        /// Retrieves a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="product">The Product object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task CreateProductAsync(Product product)
        {
            await _productRepository.CreateProductAsync(product);
        }

        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="productRequest">The Product object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateProductAsync(ProductRequestDto productRequest)
        {
            await _productRepository.UpdateProductAsync(productRequest);
        }
        
        public async Task UpdateIsDeletedAsync(ProductRequestDto productRequest)
        {
            await _productRepository.UpdateIsDeletedAsync(productRequest);
        }

        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
