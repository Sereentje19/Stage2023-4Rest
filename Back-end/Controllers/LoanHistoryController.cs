using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public IActionResult GetLoanHistory()
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var loanHistory = _loanHistoryService.GetAll();

                var pagedloanHistory = loanHistory
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

                return Ok(pagedloanHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("{productId}")]
        public IActionResult getByProductId(int productId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var loanHistory = _loanHistoryService.GetByProductId(productId);

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

        [HttpGet("ReturnDate/{productId}")]
        public IActionResult GetReturnDatesByProductId(int productId)
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

        [HttpGet("Recent/{productId}")]
        public IActionResult GetFirstByProductId(int productId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var loanHistory = _loanHistoryService.GetFirstByProductId(productId);

                var pagedLoanHistory = new
                {
                    loanHistory.LoanDate,
                    loanHistory.ReturnDate,
                    loanHistory.Customer,
                    loanHistory.LoanHistoryId,
                    loanHistory.Product,
                };

                return Ok(pagedLoanHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult ReturnProduct(LoanHistory loanHistory)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                _loanHistoryService.ReturnProduct(loanHistory);
                return Ok(new { message = "Product terug gebracht." });
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
                return Ok(new { message = "Product uitgeleend." });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}