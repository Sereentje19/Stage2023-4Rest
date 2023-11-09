using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Controllers
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
            jwtValidationService.ValidateToken(HttpContext);
            IEnumerable<LoanHistoryDTO> loanHistory = _loanHistoryService.GetLoanHistoryByProductId(productId);
            return Ok(loanHistory);
        }

        [HttpGet("customer/{customer-id}")]
        public IActionResult GetLoanHistoryByCustomerId([FromRoute(Name = "customer-id")] int customerId)
        {
            jwtValidationService.ValidateToken(HttpContext);
            IEnumerable<LoanHistoryDTO> lh = _loanHistoryService.GetLoanHistoryByCustomerId(customerId);
            return Ok(lh);
        }

        [HttpGet("date/{product-id}")]
        public IActionResult GetReturnDatesByProductId([FromRoute(Name = "product-id")] int productId)
        {
            jwtValidationService.ValidateToken(HttpContext);
            DateTime? returnDateList = _loanHistoryService.GetReturnDatesByProductId(productId);
            return Ok(returnDateList);
        }

        [HttpGet("recent/{product-id}")]
        public IActionResult GetLatestLoanHistoryByProductId([FromRoute(Name = "product-id")] int productId)
        {
            jwtValidationService.ValidateToken(HttpContext);
            LoanHistory loanHistory = _loanHistoryService.GetLatestLoanHistoryByProductId(productId);
            return Ok(loanHistory);
        }

        [HttpPut]
        public IActionResult UpdateLoanHistory(LoanHistory loanHistory)
        {
            jwtValidationService.ValidateToken(HttpContext);
            _loanHistoryService.UpdateLoanHistory(loanHistory);
            return Ok(new { message = "Product updated" });
        }

        [HttpPost]
        public IActionResult PostLoanHistory([FromBody] LoanHistory? lh)
        {
            jwtValidationService.ValidateToken(HttpContext);
            _loanHistoryService.PostLoanHistory(lh);
            return Ok(new { message = "Product created" });
        }
    }
}