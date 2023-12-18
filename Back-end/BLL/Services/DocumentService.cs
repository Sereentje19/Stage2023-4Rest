using Azure;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

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
        public async Task<IEnumerable<DocumentType>> GetDocumentTypesAsync()
        {
            return await _documentRepository.GetDocumentTypesAsync();
        }
    
        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public async Task<DocumentResponse> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetDocumentByIdAsync(id);
        }
        

        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public async Task CreateDocumentAsync(Document document)
        {
            await _documentRepository.CreateDocumentAsync(document);
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public async Task UpdateDocumentAsync(EditDocumentRequest document)
        {
            await _documentRepository.UpdateDocumentAsync(document);
        }

        /// <summary>
        /// Updates the archival status of a document based on the provided information.
        /// </summary>
        /// <param name="entity">The CheckBoxDTO containing update information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateIsArchivedAsync(CheckBoxRequest entity)
        {
            await _documentRepository.UpdateIsArchivedAsync(entity);
        }

        /// <summary>
        /// Deletes a document based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteDocumentAsync(int id)
        {
            await _documentRepository.DeleteDocumentAsync(id);
        }
    }
}
