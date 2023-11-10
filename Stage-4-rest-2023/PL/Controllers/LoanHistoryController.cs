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
    [Authorize]
    public class LoanHistoryController : ControllerBase
    {
        private readonly ILoanHistoryService _loanHistoryService;

        public LoanHistoryController(ILoanHistoryService loanHistoryService)
        {
            _loanHistoryService = loanHistoryService;
        }

        /// <summary>
        /// Retrieves loan history based on the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve loan history for.</param>
        /// <returns>
        /// ActionResult with a JSON response containing loan history details for the specified product.
        /// </returns>
        [HttpGet("product/{product-id}")]
        public async Task<IActionResult> getLoanHistoryByProductId([FromRoute(Name = "product-id")] int productId)
        {
            IEnumerable<LoanHistoryDTO> loanHistory = await _loanHistoryService.GetLoanHistoryByProductId(productId);
            return Ok(loanHistory);
        }

        /// <summary>
        /// Retrieves loan history based on the specified customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve loan history for.</param>
        /// <returns>
        /// ActionResult with a JSON response containing loan history details for the specified customer.
        /// </returns>
        [HttpGet("customer/{customer-id}")]
        public async Task<IActionResult> GetLoanHistoryByCustomerId([FromRoute(Name = "customer-id")] int customerId)
        {
            IEnumerable<LoanHistoryDTO> lh = await _loanHistoryService.GetLoanHistoryByCustomerId(customerId);
            return Ok(lh);
        }

        /// <summary>
        /// Retrieves the return dates of a product based on the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve return dates for.</param>
        /// <returns>
        /// ActionResult with a JSON response containing the return dates for the specified product.
        /// </returns>
        [HttpGet("date/{product-id}")]
        public async Task<IActionResult> GetReturnDatesByProductId([FromRoute(Name = "product-id")] int productId)
        {
            DateTime? returnDateList = await _loanHistoryService.GetReturnDatesByProductId(productId);
            return Ok(returnDateList);
        }

        /// <summary>
        /// Retrieves the latest loan history for a product based on the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve the latest loan history for.</param>
        /// <returns>
        /// ActionResult with a JSON response containing the latest loan history for the specified product.
        /// </returns>
        [HttpGet("recent/{product-id}")]
        public async Task<IActionResult> GetLatestLoanHistoryByProductId([FromRoute(Name = "product-id")] int productId)
        {
            LoanHistory loanHistory = await _loanHistoryService.GetLatestLoanHistoryByProductId(productId);
            return Ok(loanHistory);
        }

        /// <summary>
        /// Creates a new loan history entry.
        /// </summary>
        /// <param name="lh">The LoanHistory object containing information for the new entry.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostLoanHistory([FromBody] LoanHistory? lh)
        {
            await _loanHistoryService.PostLoanHistory(lh);
            return Ok(new { message = "Product created" });
        }

        /// <summary>
        /// Updates loan history information.
        /// </summary>
        /// <param name="loanHistory">The LoanHistory object containing updated information.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLoanHistory(LoanHistory loanHistory)
        {
            await _loanHistoryService.UpdateLoanHistory(loanHistory);
            return Ok(new { message = "Product updated" });
        }
    }
}