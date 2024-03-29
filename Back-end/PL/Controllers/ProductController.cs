﻿using BLL.Interfaces;
using BLL.Services;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves a paged list of products based on specified search criteria and pagination parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for product details.</param>
        /// <param name="dropdown">The product type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns>
        /// ActionResult with a JSON response containing paged products and pagination details.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string searchfield, string dropdown, int page = 1, int pageSize = 5)
        {
            (IEnumerable<object> pagedProducts, Pager pager) = await _productService.GetAllProducts(searchfield, dropdown, page, pageSize);

            var response = new
            {
                Products = pagedProducts,
                Pager = new
                {
                    pager.TotalItems,
                    pager.CurrentPage,
                    pager.PageSize,
                    pager.TotalPages,
                }
            };

            return Ok(response);
        }
        
        /// <summary>
        /// Retrieves a paginated list of all deleted products based on the specified search criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for filtering products.</param>
        /// <param name="dropdown">The dropdown selection for filtering products.</param>
        /// <param name="page">The page number for paginated results (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 5).</param>
        /// <returns>An IActionResult containing a response with paginated deleted products and pager information.</returns>
        [HttpGet("deleted")]
        public async Task<IActionResult> GetAllDeletedProducts(string searchfield, string dropdown, int page = 1, int pageSize = 5)
        {
            (IEnumerable<object> pagedProducts, Pager pager) = await _productService.GetAllDeletedProducts(searchfield, dropdown, page, pageSize);

            var response = new
            {
                Products = pagedProducts,
                Pager = new
                {
                    pager.TotalItems,
                    pager.CurrentPage,
                    pager.PageSize,
                    pager.TotalPages,
                }
            };

            return Ok(response);
        }
        
        /// <summary>
        /// Retrieves a list of product type strings.
        /// </summary>
        /// <returns>
        /// a list of strings representing product types.
        /// </returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypesAsync()
        {
            IEnumerable<ProductType> productType = await _productService.GetProductTypesAsync();
            return Ok(productType);
        }

        /// <summary>
        /// Retrieves a product based on the specified product ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>
        /// ActionResult with a JSON response containing details of the specified product.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            Product product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// Creates a new product entry.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="product"></param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromForm] IFormFile file, [FromForm] ProductRequestDto product)
        {
            Product pro = new Product
            {
                Type = product.Type,
                FileType = product.FileType,
                PurchaseDate = product.PurchaseDate,
                ExpirationDate = product.ExpirationDate,
                SerialNumber = product.SerialNumber
            };
            
            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    pro.File = memoryStream.ToArray();
                }
            }
            
            await _productService.CreateProductAsync(pro);
            return Ok(new { message = "Product toegevoegd." });
        }

        /// <summary>
        /// Updates product information.
        /// </summary>
        /// <param name="productRequest">The Product object containing updated information.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(ProductRequestDto productRequest)
        {
            await _productService.UpdateProductAsync(productRequest);
            return Ok(new { message = "Product geupdate." });
        }
        
        /// <summary>
        /// Updates the IsDeleted status of a product based on the provided ProductRequestDto.
        /// </summary>
        /// <param name="productRequest">The ProductRequestDto containing information for updating the product.</param>
        /// <returns>An IActionResult indicating the success of the operation.</returns>
        [HttpPut("delete")]
        public async Task<IActionResult> UpdateIsDeletedAsync(ProductRequestDto productRequest)
        {
            await _productService.UpdateIsDeletedAsync(productRequest);
            return Ok(new { message = "Product geupdate." });
        }

        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(new { message = "Product verwijderd." });
        }
    }
}
