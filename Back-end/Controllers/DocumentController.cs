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


        // allDocuments.Sort((y, x) => y.Date.CompareTo(x.Date));

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 5)
        {
            List<Models.Document> allDocuments = documentService.GetAll();

            // Calculate the number of documents to skip based on the page and pageSize
            int skipCount = (page - 1) * pageSize;

            // Get the paginated subset of documents
            var pagedDocuments = allDocuments
                .OrderBy(doc => doc.Date)
                .Skip(skipCount) // Skip the appropriate number of documents
                .Take(pageSize) // Take the specified number of documents
                .Select(doc => new
                {
                    doc.DocumentId,
                    doc.Image,
                    doc.Date,
                    doc.CustomerId,
                    Type = doc.Type.ToString()
                })
                .ToList();

            // Create a Pager instance to calculate paging information
            var pager = new Pager(allDocuments.Count, page, pageSize);

            // You can return both the paged documents and the pager information in the response
            var response = new
            {
                Documents = pagedDocuments,
                Pager = new
                {
                    pager.TotalItems,
                    pager.CurrentPage,
                    pager.PageSize,
                    pager.TotalPages,
                    pager.StartPage,
                    pager.EndPage,
                    pager.StartIndex,
                    pager.EndIndex,
                    Pages = pager.Pages.ToList()
                }
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            jwtValidationService.ValidateToken(HttpContext);
            var document = documentService.GetById(id);
            return Ok(document);
        }

        [HttpPost]
        public IActionResult Post([FromForm] IFormFile file, [FromForm] Models.Document document)
        {
            // jwtValidationService.ValidateToken(HttpContext);

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

        [HttpPut]
        public IActionResult Put(Models.Document doc)
        {
            jwtValidationService.ValidateToken(HttpContext);
            documentService.Put(doc);
            return Ok(new { message = "Document updated" });
        }

        [HttpDelete]
        public IActionResult Delete(Models.Document doc)
        {
            jwtValidationService.ValidateToken(HttpContext);
            documentService.Delete(doc);
            return Ok(new { message = "Document Deleted" });
        }


    }
}