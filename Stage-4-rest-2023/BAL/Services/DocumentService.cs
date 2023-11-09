using Stage4rest2023.Repositories;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        /// <summary>
        /// Initializes a new instance of the DocumentService class with the provided DocumentRepository.
        /// </summary>
        /// <param name="dr">The DocumentRepository used for document-related operations.</param>
        public DocumentService(IDocumentRepository dr)
        {
            _documentRepository = dr;
        }

        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            var (documentList, numberOfDocuments) = _documentRepository.GetPagedDocuments(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfDocuments, page, pageSize);
            return (documentList, pager);
        }
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            var (documentList, numberOfDocuments) = _documentRepository.GetArchivedPagedDocuments(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfDocuments, page, pageSize);
            return (documentList, pager);
        }
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
        public DocumentDTO GetDocumentById(int id)
        {
            return _documentRepository.GetDocumentById(id);
        }

        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public void PostDocument(Document document)
        {
            _documentRepository.AddDocument(document);
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public void PutDocument(EditDocumentRequestDTO document)
        {
            _documentRepository.UpdateDocument(document);
        }

        public void UpdateIsArchived(CheckBoxDTO entity)
        {
            _documentRepository.UpdateIsArchived(entity);
        }

        public void DeleteDocument(int id)
        {
            _documentRepository.DeleteDocument(id);
        }
    }
}