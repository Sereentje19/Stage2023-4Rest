using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;
        private readonly IJwtValidationService jwtValidationService;

        public FilterController(IFilterService fs, IJwtValidationService jwt)
        {
            _filterService = fs;
            jwtValidationService = jwt;
        }

        // [HttpGet("documents-and-customers")]
        // public IActionResult FilterDocumentsAndCustomers(string? searchfield, Models.Type? dropBoxType, int page = 1, int pageSize = 5)
        // {
        //     try
        //     {
        //         jwtValidationService.ValidateToken(HttpContext);
        //         var (pagedDocuments, pager) = _filterService.FilterDocumentsAndCustomers(searchfield, dropBoxType, page, pageSize);

        //         var response = new
        //         {
        //             Documents = pagedDocuments,
        //             Pager = new
        //             {
        //                 pager.TotalItems,
        //                 pager.CurrentPage,
        //                 pager.PageSize,
        //                 pager.TotalPages,
        //             }
        //         };

        //         return Ok(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(401, ex.Message);
        //     }
        // }
    }
}