using Microsoft.AspNetCore.Mvc;
using Back_end.Services;
using Microsoft.AspNetCore.Cors;
using Back_end.Models;
using Back_end.Models.DTOs;

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

        [HttpGet]
        public IActionResult GetAllDocuments()
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var documents = documentService.GetAll();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("Filter")]
        public IActionResult GetFilterDocuments(string? searchfield, string overviewType, DocumentType? dropBoxType, int page = 1, int pageSize = 5)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var (pagedDocuments, pager) = documentService.GetFilterDocuments(searchfield, dropBoxType, page, pageSize, overviewType);

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
                var response = documentService.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [HttpGet("customerId/{customerId}")]
        public IActionResult GetByCustomerId(int customerId)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                var documents = documentService.GetByCustomerId(customerId);
                return Ok(documents);
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
        public IActionResult Post([FromForm] IFormFile? file, [FromForm] DocumentDTO document)
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

                if (file != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        doc.File = memoryStream.ToArray();
                    }
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
        public IActionResult Put(EditDocumentRequestDTO doc)
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

        [HttpPut("IsArchived")]
        public IActionResult PutIsArchived(CheckBoxDTO doc)
        {
            try
            {
                jwtValidationService.ValidateToken(HttpContext);
                documentService.UpdateIsArchived(doc);
                return Ok(new { message = "Document updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }
    }
}