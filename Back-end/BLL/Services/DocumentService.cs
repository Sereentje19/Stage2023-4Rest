using Azure;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;

namespace BLL.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Retrieves a paged list of documents based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>A tuple containing paged documents and pagination information.</returns>
        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, string dropdown, int page, int pageSize)
        {
            (IEnumerable<object> documentList, int numberOfDocuments) = _documentRepository.GetPagedDocuments(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfDocuments, page, pageSize);
            return (documentList, pager);
        }

        /// <summary>
        /// Retrieves a paged list of archived documents based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>A tuple containing paged documents and pagination information.</returns>
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, string dropdown, int page, int pageSize)
        {
            (IEnumerable<object> documentList, int numberOfDocuments) = _documentRepository.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfDocuments, page, pageSize);
            return (documentList, pager);
        }

        /// <summary>
        /// Retrieves a paged list of long-valid documents based on the specified search field.
        /// </summary>
        /// <param name="searchfield">The search criteria.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>A tuple containing paged documents and pagination information.</returns>
        public (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, string dropdown, int page, int pageSize)
        {
            (IEnumerable<object> documentList, int numberOfDocuments) = _documentRepository.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfDocuments, page, pageSize);
            return (documentList, pager);
        }

        /// <summary>
        /// Retrieves a list of document type strings from the underlying document repository.
        /// </summary>
        /// <returns>
        /// A list of strings representing document types.
        /// </returns>
        public Task<IEnumerable<DocumentType>> GetDocumentTypesAsync()
        {
            return _documentRepository.GetDocumentTypesAsync();
        }
    
        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public Task<DocumentResponseDto> GetDocumentByIdAsync(int id)
        {
            return _documentRepository.GetDocumentByIdAsync(id);
        }
        

        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public Task CreateDocumentAsync(Document document)
        {
            ValidationHelper.ValidateObject(document);
            
            if (string.IsNullOrWhiteSpace(document.Employee.Name))
            {
                throw new InputValidationException("Klant naam is leeg.");
            }

            if (string.IsNullOrWhiteSpace(document.Employee.Email) || !document.Employee.Email.Contains("@"))
            {
                throw new InputValidationException("Geen geldige email.");
            }
            
            if (document.Type.Name == "0")
            {
                throw new InputValidationException("Selecteer een type.");
            }

            if (document.Date < DateTime.Today)
            {
                throw new InputValidationException("Datum is incorrect, de datum moet in de toekomst zijn.");
            }
            
            return _documentRepository.CreateDocumentAsync(document);
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public Task UpdateDocumentAsync(EditDocumentRequestDto document)
        {
            ValidationHelper.ValidateObject(document);
            
            if (document.Type.Name == "0")
            {
                throw new InputValidationException("Selecteer een type.");
            }
            
            return _documentRepository.UpdateDocumentAsync(document);
        }

        /// <summary>
        /// Updates the archival status of a document based on the provided information.
        /// </summary>
        /// <param name="entity">The CheckBoxDTO containing update information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task UpdateIsArchivedAsync(CheckBoxRequestDto entity)
        {
            ValidationHelper.ValidateObject(entity);
            return _documentRepository.UpdateIsArchivedAsync(entity);
        }

        /// <summary>
        /// Deletes a document based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public Task DeleteDocumentAsync(int id)
        {
            return _documentRepository.DeleteDocumentAsync(id);
        }
    }
}
