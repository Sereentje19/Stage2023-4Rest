using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Exceptions;
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
        
        /// <summary>
        /// Retrieves a collection of deleted products based on the specified search criteria and paging parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering products.</param>
        /// <param name="dropdown">The dropdown value for additional filtering.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>
        /// A tuple containing the collection of deleted products and pagination information represented by a <see cref="Pager"/>.
        /// </returns>
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
        public Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return _productRepository.GetProductTypesAsync();
        }
        
        /// <summary>
        /// Retrieves a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public Task<Product> GetProductByIdAsync(int id)
        {
            if (id == 0)
            {
                throw new NotFoundException("Oeps, er gaat iets fout! Het product kan niet worden opgehaald.");
            }

            return _productRepository.GetProductByIdAsync(id);
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="product">The Product object to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task CreateProductAsync(Product product)
        {
            ValidationHelper.ValidateObject(product);
            
            if (product.Type.Name == "0")
            {
                throw new InputValidationException("Type is leeg.");
            }

            if (product.ExpirationDate < DateTime.Today || product.PurchaseDate < DateTime.Today)
            {
                throw new InputValidationException("Datum is incorrect, de datum moet in de toekomst zijn.");
            }
            
            if (string.IsNullOrWhiteSpace(product.SerialNumber))
            {
                throw new InputValidationException("Serie nummer is leeg.");
            }
            
            return _productRepository.CreateProductAsync(product);
        }

        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="productRequest">The Product object containing updated information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task UpdateProductAsync(ProductRequestDto productRequest)
        {
            ValidationHelper.ValidateObject(productRequest);
            
            if (productRequest.ProductId == 0)
            {
                throw new NotFoundException("Oeps, er gaat iets fout! Het product kan niet geupdate worden.");
            }

            return _productRepository.UpdateProductAsync(productRequest);
        }
        
        public Task UpdateIsDeletedAsync(ProductRequestDto productRequest)
        {
            ValidationHelper.ValidateObject(productRequest);
            
            if (productRequest.ProductId == 0)
            {
                throw new NotFoundException("Oeps, er gaat iets fout! Het product kan niet geupdate worden.");
            }

            return _productRepository.UpdateIsDeletedAsync(productRequest);
        }

        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task DeleteProductAsync(int id)
        {
            if (id == 0)
            {
                throw new NotFoundException("Oeps, er gaat iets fout! Het product kan niet worden verwijderd.");
            }

            return _productRepository.DeleteProductAsync(id);
        }
    }
}
