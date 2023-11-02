using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("loan-history")]
    public class LoanHistoryController : ControllerBase
    {
        private readonly ILoanHistoryService _loanHistoryService;
        private readonly IJwtValidationService jwtValidationService;

        /// <summary>
        /// Initializes a new instance of the CustomerController class.
        /// </summary>
        /// <param name="cs">The customer service for managing customers.</param>
        /// <param name="jwt">The JWT validation service for token validation.</param>
        public LoanHistoryController(ILoanHistoryService loanHistoryService, IJwtValidationService jwt)
        {
            _loanHistoryService = loanHistoryService;
            jwtValidationService = jwt;
        }

        [HttpGet("product/{product-id}")]
        public IActionResult getLoanHistoryByProductId([FromRoute(Name = "product-id")] int productId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var loanHistory = _loanHistoryService.GetLoanHistoryByProductId(productId);

                var pagedLoanHistory = loanHistory
                .Select(loan => new
                {
                    Type = loan.Product.Type.ToString(),
                    loan.Product.SerialNumber,
                    loan.Product.ProductId,
                    loan.Product.ExpirationDate,
                    loan.Product.PurchaseDate,
                    loan.LoanDate,
                    loan.ReturnDate,
                    loan.Customer.Name
                })
                .ToList();

                return Ok(pagedLoanHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("customer/{customer-id}")]
        public IActionResult GetLoanHistoryByCustomerId([FromRoute(Name = "customer-id")] int customerId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var lh = _loanHistoryService.GetLoanHistoryByCustomerId(customerId);

                var LoanHistory = lh
                .Select(loan => new
                {
                    Type = loan.Product.Type.ToString(),
                    loan.Product.SerialNumber,
                    loan.LoanDate,
                    loan.ReturnDate,
                    loan.Customer.Name,
                    loan.Customer.Email
                })
                .ToList();

                return Ok(LoanHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("date/{product-id}")]
        public IActionResult GetReturnDatesByProductId([FromRoute(Name = "product-id")] int productId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var returnDateList = _loanHistoryService.GetReturnDatesByProductId(productId);
                return Ok(returnDateList);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("recent/{product-id}")]
        public IActionResult GetLatestLoanHistoryByProductId([FromRoute(Name = "product-id")] int productId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var loanHistory = _loanHistoryService.GetLatestLoanHistoryByProductId(productId);
                return Ok(loanHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateLoanHistory(LoanHistory loanHistory)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                _loanHistoryService.UpdateLoanHistory(loanHistory);
                return Ok(new { message = "Product updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PostLoanHistory([FromBody] LoanHistory? lh)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                _loanHistoryService.PostLoanHistory(lh);
                return Ok(new { message = "Product created" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}