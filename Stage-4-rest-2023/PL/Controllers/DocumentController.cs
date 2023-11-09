using Microsoft.AspNetCore.Mvc;
using Stage4rest2023.Services;
using Microsoft.AspNetCore.Cors;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("document")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;
        private readonly IJwtValidationService jwtValidationService;

        /// <summary>
        /// Initializes a new instance of the DocumentController class.
        /// </summary>
        /// <param name="ds">The document service for managing documents.</param>
        /// <param name="jwt">The JWT validation service for token validation.</param>
        public DocumentController(IDocumentService ds, IJwtValidationService jwt)
        {
            documentService = ds;
            jwtValidationService = jwt;
        }

        [HttpGet]
        public IActionResult GetFilteredPagedDocuments(string? searchfield, DocumentType? dropdown,
            int page = 1, int pageSize = 5)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            var (pagedDocuments, pager) = documentService.GetPagedDocuments(searchfield, dropdown, page, pageSize);

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

        [HttpGet("archive")]
        public IActionResult GetArchivedPagedDocuments(string? searchfield, DocumentType? dropdown,
            int page = 1, int pageSize = 5)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            var (pagedDocuments, pager) =
                documentService.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);

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

        [HttpGet("long-valid")]
        public IActionResult GetLongValidPagedDocuments(string? searchfield, DocumentType? dropdown,
            int page = 1, int pageSize = 5)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            var (pagedDocuments, pager) =
                documentService.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize);

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

        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document and its type information if found; otherwise, an error message.</returns>
        [HttpGet("{id}")]
        public IActionResult GetDocumentById(int id)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            DocumentDTO doc = documentService.GetDocumentById(id);
            
            var response = new
            {
                Document = doc,
                doc.Customer,
                type = doc.Type.ToString().Replace("_", " "),
            };
            
            return Ok(response);
        }

        /// <summary>
        /// Uploads a new document and associates it with the provided document information.
        /// </summary>
        /// <param name="file">The document file to upload.</param>
        /// <param name="document">The document information, including type, date, and customer ID.</param>
        /// <returns>A success message if the document is created; otherwise, an error message.</returns>
        [HttpPost]
        public IActionResult PostDocument([FromForm] IFormFile? file, [FromForm] DocumentDTO document)
        {
            // jwtValidationService.ValidateToken(HttpContext);

            Document doc = new Document
            {
                Type = document.Type,
                Date = document.Date,
                Customer = new Customer()
                {
                    Email = document.Customer.Email,
                    Name = document.Customer.Name
                },
                FileType = document.FileType
            };

            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    doc.File = memoryStream.ToArray();
                }
            }

            documentService.PostDocument(doc);
            return Ok(new { message = "Document created" });
        }

        /// <summary>
        /// Updates an existing document with the provided information.
        /// </summary>
        /// <param name="doc">The document entity to be updated.</param>
        /// <returns>A success message if the document is updated; otherwise, an error message.</returns>
        [HttpPut]
        public IActionResult PutDocument(EditDocumentRequestDTO doc)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            documentService.PutDocument(doc);
            return Ok(new { message = "Document updated" });
        }

        [HttpPut("archive")]
        public IActionResult PutIsArchived(CheckBoxDTO doc)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            documentService.UpdateIsArchived(doc);
            return Ok(new { message = "Document updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument(int id)
        {
            // jwtValidationService.ValidateToken(HttpContext);
            documentService.DeleteDocument(id);
            return Ok(new { message = "Document deleted" });
        }
    }
}