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
            List<Models.Document> allDocuments = documentService.GetAll();
            // int skipCount = (page - 1) * pageSize;
            // var pager = new Pager(allDocuments.Count, page, pageSize);

            IEnumerable<Models.Document> filteredDocuments;
            DateTime currentDate = DateTime.Now;

            if (isArchived)
            {
                filteredDocuments = allDocuments.Where(doc => doc.Date < currentDate)
                .OrderByDescending(doc => doc.Date);
            }
            else
            {
                filteredDocuments = allDocuments.Where(doc => doc.Date > currentDate)
                .OrderBy(doc => doc.Date);
            }

            int totalArchivedDocuments = filteredDocuments.Count();

    // Calculate skipCount based on the total number of archived documents
    int skipCount = Math.Max(0, (page - 1) * pageSize);

    var pager = new Pager(totalArchivedDocuments, page, pageSize);

            var pagedDocuments = filteredDocuments
                .Skip(skipCount)
                .Take(pageSize)
                .Select(doc => new
                {
                    doc.DocumentId,
                    doc.Image,
                    doc.Date,
                    doc.CustomerId,
                    Type = doc.Type.ToString()
                })
                .ToList();

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




        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            var document = documentService.GetById(id);

            var response = new
            {
                Document = document,
                type = document.Type.ToString()
            };

            return Ok(response);
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
            // jwtValidationService.ValidateToken(HttpContext);
            documentService.Put(doc);
            return Ok(new { message = "Document updated" });
        }

        [HttpDelete]
        public IActionResult Delete(Models.Document doc)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            documentService.Delete(doc);
            return Ok(new { message = "Document Deleted" });
        }


    }
}