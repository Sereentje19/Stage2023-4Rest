using Microsoft.AspNetCore.Mvc;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using Back_end.Models;

namespace Back_end.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("[controller]")]
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

        /// <summary>
        /// Retrieves a paged list of documents based on specified parameters.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 5).</param>
        /// <param name="isArchived">A flag indicating whether to retrieve archived documents (default is false).</param>
        /// <returns>A collection of documents and pagination information.</returns>
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

        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document and its type information if found; otherwise, an error message.</returns>
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
                    type = document.Type.ToString().Replace("_", " "),
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        /// <summary>
        /// Uploads a new document and associates it with the provided document information.
        /// </summary>
        /// <param name="file">The document file to upload.</param>
        /// <param name="document">The document information, including type, date, and customer ID.</param>
        /// <returns>A success message if the document is created; otherwise, an error message.</returns>
        [HttpPost]
        public IActionResult Post([FromForm] IFormFile file, [FromForm] Document document)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                Document doc = new Document
                {
                    Type = document.Type,
                    Date = document.Date,
                    CustomerId = document.CustomerId,
                    FileType = document.FileType
                };

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    doc.File = memoryStream.ToArray();
                }

                documentService.Post(doc);
                return Ok(new { message = "Document created" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing document with the provided information.
        /// </summary>
        /// <param name="doc">The document entity to be updated.</param>
        /// <returns>A success message if the document is updated; otherwise, an error message.</returns>
        [HttpPut]
        public IActionResult Put(Document doc)
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