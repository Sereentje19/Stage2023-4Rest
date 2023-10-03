using System;
using Microsoft.AspNetCore.Mvc;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using System.Reflection.Metadata;
using Back_end.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Drawing;

namespace Back_end.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;
        private readonly IJwtValidationService jwtValidationService;

        public DocumentController(IDocumentService ds, IJwtValidationService jwtv)
        {
            documentService = ds;
            jwtValidationService = jwtv;
        }

        [HttpGet]
        public IActionResult GetDocuments(int page = 1, int pageSize = 5, bool isArchived = false)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var (pagedDocuments, pager) = documentService.GetAllPagedDocuments(isArchived, page, pageSize);

                var response = new
                {
                    Documents = pagedDocuments,
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
        public IActionResult GetById(int id)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var document = documentService.GetById(id);

                var response = new
                {
                    Document = document,
                    type = document.Type.ToString()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromForm] IFormFile file, [FromForm] Models.Document document)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);

                Models.Document doc = new Models.Document
                {
                    Type = document.Type,
                    Date = document.Date,
                    CustomerId = document.CustomerId
                };

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    doc.Image = memoryStream.ToArray();
                }

                documentService.Post(doc);
                return Ok(new { message = "Document created" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Models.Document doc)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                documentService.Put(doc);
                return Ok(new { message = "Document updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}