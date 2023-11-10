using Stage4rest2023.Repositories;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
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
        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            var (documentList, numberOfDocuments) = _documentRepository.GetPagedDocuments(searchfield, dropdown, page, pageSize);
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
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            var (documentList, numberOfDocuments) = _documentRepository.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);
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
        public (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            var (documentList, numberOfDocuments) = _documentRepository.GetLongValidPagedDocuments(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfDocuments, page, pageSize);
            return (documentList, pager);
        }

        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public async Task<DocumentDTO> GetDocumentById(int id)
        {
            return await _documentRepository.GetDocumentById(id);
        }

        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public async Task PostDocument(Document document)
        {
            await _documentRepository.AddDocument(document);
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public async Task PutDocument(EditDocumentRequestDTO document)
        {
            await _documentRepository.UpdateDocument(document);
        }

        /// <summary>
        /// Updates the archival status of a document based on the provided information.
        /// </summary>
        /// <param name="entity">The CheckBoxDTO containing update information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateIsArchived(CheckBoxDTO entity)
        {
            await _documentRepository.UpdateIsArchived(entity);
        }

        /// <summary>
        /// Deletes a document based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteDocument(int id)
        {
            await _documentRepository.DeleteDocument(id);
        }
    }
}