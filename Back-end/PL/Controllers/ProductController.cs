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
        public async Task<IActionResult> getAllProducts(string searchfield, string dropdown, int page = 1, int pageSize = 5)
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
        /// Retrieves a list of product type strings.
        /// </summary>
        /// <returns>
        /// a list of strings representing product types.
        /// </returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypes()
        {
            IEnumerable<ProductType> productType = await _productService.GetProductTypes();
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
        public async Task<IActionResult> GetProductById(int id)
        {
            Product product = await _productService.GetProductById(id);
            return Ok(product);
        }

        /// <summary>
        /// Creates a new product entry.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="product">The Product object containing information for the new entry.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] IFormFile file, [FromForm] Product product)
        {
            Console.WriteLine(product.Type.Name);
            
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
            
            await _productService.PostProduct(pro);
            return Ok(new { message = "Product toegevoegd." });
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new { message = "Product verwijderd." });
        }
    }
}
