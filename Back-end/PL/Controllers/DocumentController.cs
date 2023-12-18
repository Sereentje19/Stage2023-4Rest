using BLL.Interfaces;
using BLL.Services;
using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PL.Attributes;

namespace PL.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("document")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Retrieves a paged list of documents based on specified search criteria and pagination parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for document details.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>
        /// ActionResult with a JSON response containing paged documents and pagination details.
        /// </returns>
        [HttpGet]
        public IActionResult GetFilteredPagedDocuments(string searchfield, string dropdown,
            int page = 1, int pageSize = 5)
        {
            (IEnumerable<object> pagedDocuments, Pager pager) = _documentService.GetPagedDocuments(searchfield, dropdown, page, pageSize);

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
        /// Retrieves a paged list of archived documents based on specified search criteria and pagination parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for archived document details.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of archived documents per page.</param>
        /// <returns>
        /// ActionResult with a JSON response containing paged archived documents and pagination details.
        /// </returns>
        [HttpGet("archive")]
        public IActionResult GetArchivedPagedDocuments(string searchfield, string dropdown,
            int page = 1, int pageSize = 5)
        {
            (IEnumerable<object> pagedDocuments, Pager pager) =
                _documentService.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);

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
        /// Retrieves a paged list of long-valid documents based on specified search criteria and pagination parameters.
        /// </summary>
        /// <param name="searchfield">The search criteria for long-valid document details.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of long-valid documents per page.</param>
        /// <returns>
        /// ActionResult with a JSON response containing paged long-valid documents and pagination details.
        /// </returns>
        [HttpGet("long-valid")]
        public IActionResult GetLongValidPagedDocuments(string searchfield, string dropdown,
            int page = 1, int pageSize = 5)
        {
            (IEnumerable<object> pagedDocuments, Pager pager) =
                _documentService.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize);

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
        /// Retrieves a list of document type strings.
        /// </summary>
        /// <returns>
        /// a list of strings representing document types.
        /// </returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetDocumentTypesAsync()
        {
            IEnumerable<DocumentType> documentTypes = await _documentService.GetDocumentTypesAsync();
            return Ok(documentTypes);
        }
        
        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document and its type information if found; otherwise, an error message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentByIdAsync(int id)
        {
            DocumentResponseDto document = await _documentService.GetDocumentByIdAsync(id);
            return Ok(document);
        }

        /// <summary>
        /// Uploads a new document and associates it with the provided document information.
        /// </summary>
        /// <param name="file">The document file to upload.</param>
        /// <param name="document">The document information, including type, date, and customer ID.</param>
        /// <returns>A success message if the document is created; otherwise, an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDocumentAsync([FromForm] IFormFile file, [FromForm] DocumentResponseDto document)
        {
            Document doc = new Document
            {
                Type = document.Type,
                Date = document.Date,
                Employee = new Employee()
                {
                    Email = document.Employee.Email,
                    Name = document.Employee.Name
                },
                FileType = document.FileType
            };

            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    doc.File = memoryStream.ToArray();
                }
            }

            await _documentService.CreateDocumentAsync(doc);
            return Ok(new { message = "Document toegevoegd." });
        }

        /// <summary>
        /// Updates an existing document with the provided information.
        /// </summary>
        /// <param name="doc">The document entity to be updated.</param>
        /// <returns>A success message if the document is updated; otherwise, an error message.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateDocumentAsync(EditDocumentRequestDto doc)
        {
            await _documentService.UpdateDocumentAsync(doc);
            return Ok(new { message = "Document geupdate." });
        }

        /// <summary>
        /// Updates the archived status of a document based on the provided CheckBoxDTO.
        /// </summary>
        /// <param name="doc">The CheckBoxDTO containing document information and archived status.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpPut("archive")]
        public async Task<IActionResult> UpdateIsArchivedAsync(CheckBoxRequestDto doc)
        {
            await _documentService.UpdateIsArchivedAsync(doc);
            return Ok(new { message = "Document geupdate." });
        }

        /// <summary>
        /// Deletes a document based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>
        /// ActionResult with a JSON response indicating the success of the operation.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentAsync(int id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return Ok(new { message = "Document verwijderd." });
        }
    }
}
