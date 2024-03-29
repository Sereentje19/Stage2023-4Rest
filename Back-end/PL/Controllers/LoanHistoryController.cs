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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// ActionResult with a JSON response containing loan history details for the specified product.
        /// </returns>
        [HttpGet("product/{product-id}")]
        public async Task<IActionResult> GetLoanHistoryByProductIdAsync([FromRoute(Name = "product-id")] int productId, int page, int pageSize)
        {
            (IEnumerable<object> pagedHistory, Pager pager) = await _loanHistoryService.GetLoanHistoryByProductIdAsync(productId, page, pageSize);
            
            var response = new
            {
                LoanHistory = pagedHistory,
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
        /// Retrieves loan history based on the specified customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve loan history for.</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// ActionResult with a JSON response containing loan history details for the specified customer.
        /// </returns>
        [HttpGet("employee/{employee-id}")]
        public async Task<IActionResult> GetLoanHistoryByEmployeeIdAsync([FromRoute(Name = "employee-id")] int customerId, int page, int pageSize)
        {
            (IEnumerable<object> pagedHistory, Pager pager) = await _loanHistoryService.GetLoanHistoryByEmployeeIdAsync(customerId, page, pageSize);
            
            var response = new
            {
                LoanHistory = pagedHistory,
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
        /// Retrieves the return dates of a product based on the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve return dates for.</param>
        /// <returns>
        /// ActionResult with a JSON response containing the return dates for the specified product.
        /// </returns>
        [HttpGet("date/{product-id}")]
        public async Task<IActionResult> GetReturnDatesByProductIdAsync([FromRoute(Name = "product-id")] int productId)
        {
            DateTime? returnDateList = await _loanHistoryService.GetReturnDatesByProductIdAsync(productId);
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
        public async Task<IActionResult> GetLatestLoanHistoryByProductIdAsync([FromRoute(Name = "product-id")] int productId)
        {
            LoanHistory loanHistory = await _loanHistoryService.GetLatestLoanHistoryByProductIdAsync(productId);
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
        public async Task<IActionResult> CreateLoanHistoryAsync([FromBody] LoanHistoryRequestDto lh)
        {
            await _loanHistoryService.CreateLoanHistoryAsync(lh);
            return Ok(new { message = "Leen geschiedenis toegevoegd." });
        }

        /// <summary>
        /// Updates loan history information.
        /// </summary>
        /// <param name="loanHistoryRequest">The LoanHistory object containing updated information.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLoanHistoryAsync(LoanHistoryRequestDto loanHistoryRequest)
        {
            await _loanHistoryService.UpdateLoanHistoryAsync(loanHistoryRequest);
            return Ok(new { message = "Leen geschiedenis geupdate" });
        }
    }
}
