using Stage4rest2023.Models;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Stage4rest2023.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IJwtValidationService jwtValidationService;

        /// <summary>
        /// Initializes a new instance of the CustomerController class.
        /// </summary>
        /// <param name="cs">The customer service for managing customers.</param>
        /// <param name="jwt">The JWT validation service for token validation.</param>
        public ProductController(IProductService productService, IJwtValidationService jwt)
        {
            _productService = productService;
            jwtValidationService = jwt;
        }

        [HttpGet]
        public IActionResult getAllProducts(string? searchfield, ProductType? dropdown, int page = 1, int pageSize = 5)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            var (pagedProducts, pager) = _productService.GetAllProducts(searchfield, dropdown, page, pageSize);

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

        [HttpGet("{id}")]
        public IActionResult getProductById(int id)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            Product product = _productService.GetProductById(id);

            var response = new
            {
                product,
                ProductType = product.Type.ToString(),
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult PostProduct(Product product)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            _productService.PostProduct(product);
            return Ok(new { message = "Product created" });
        }

        [HttpPut]
        public IActionResult PutProduct(Product product)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            _productService.PutProduct(product);
            return Ok(new { message = "Product updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            _productService.DeleteProduct(id);
            return Ok(new { message = "Product deleted" });
        }
    }
}