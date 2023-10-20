using Back_end.Repositories;
using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
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

        public async Task<IEnumerable<Document>> GetAll()
        {
            return await _documentRepository.getAll();
        }


        public (IEnumerable<object>, Pager) GetFilterDocuments(string searchfield, DocumentType? dropBoxType, int page, int pageSize, string overviewType)
        {
            var documents = _documentRepository.GetFilterDocuments(searchfield, dropBoxType, overviewType);

            int totalArchivedDocuments = documents.Count();
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            var pager = new Pager(totalArchivedDocuments, page, pageSize);


            var pagedDocuments = documents
                .Skip(skipCount)
                .Take(pageSize)
                .Select(doc => new
                {
                    doc.DocumentId,
                    doc.Date,
                    CustomerName = doc.Customer.Name,
                    Type = doc.Type.ToString().Replace("_", " "),
                })
                .ToList();

            return (pagedDocuments.Cast<object>(), pager);
        }

        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public object GetById(int id)
        {
            var doc = _documentRepository.GetById(id);

            var response = new
            {
                Document = doc,
                Customer = doc.customer,
                type = doc.Type.ToString().Replace("_", " "),
            };
            return response;
        }

        public IEnumerable<Document> GetByCustomerId(int customerId)
        {
            return _documentRepository.GetByCustomerId(customerId);
        }

        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public void Post(Document document)
        {
            _documentRepository.Add(document);
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public void Put(EditDocumentRequestDTO document)
        {
            _documentRepository.Update(document);
        }

        public void UpdateIsArchived(CheckBoxDTO entity)
        {
            _documentRepository.UpdateIsArchived(entity);
        }

        public void UpdateCustomerId(int customerId, int documentId)
        {
            _documentRepository.UpdateCustomerId(customerId, documentId);
        }
    }
}