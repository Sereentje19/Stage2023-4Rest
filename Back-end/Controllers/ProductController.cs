using System;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var (pagedProducts, pager) = _productService.GetAll(searchfield, dropdown, page, pageSize);

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
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                Product product = _productService.GetById(id);

                var pagedproducts = new
                {
                    product.SerialNumber,
                    product.PurchaseDate,
                    product.ExpirationDate,
                    product.ProductId,
                    ProductType = product.Type.ToString(),
                    product.Type,
                };

                return Ok(pagedproducts);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Product product)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                _productService.Put(product);
                return Ok(new { message = "Product deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                _productService.Delete(id);
                return Ok(new { message = "Product deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}