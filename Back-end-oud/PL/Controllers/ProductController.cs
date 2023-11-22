using BLL.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;
using PL.Models;

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
        public async Task<IActionResult> getAllProducts(string? searchfield, ProductType? dropdown, int page = 1, int pageSize = 5)
        {
            var (pagedProducts, pager) = await _productService.GetAllProducts(searchfield, dropdown, page, pageSize);

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
        /// Retrieves a product based on the specified product ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>
        /// ActionResult with a JSON response containing details of the specified product.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> getProductById(int id)
        {
            Product product = await _productService.GetProductById(id);

            var response = new
            {
                product,
                ProductType = product.Type.ToString(),
            };

            return Ok(response);
        }

        /// <summary>
        /// Creates a new product entry.
        /// </summary>
        /// <param name="product">The Product object containing information for the new entry.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            await _productService.PostProduct(product);
            return Ok(new { message = "Product created" });
        }

        /// <summary>
        /// Updates product information.
        /// </summary>
        /// <param name="product">The Product object containing updated information.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> PutProduct(Product product)
        {
            await _productService.PutProduct(product);
            return Ok(new { message = "Product updated" });
        }

        /// <summary>
        /// Deletes a product based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new { message = "Product deleted" });
        }
    }
}
