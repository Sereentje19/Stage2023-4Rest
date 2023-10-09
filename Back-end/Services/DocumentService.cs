using System;
using Back_end.Repositories;
using Back_end.Models;

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

        /// <summary>
        /// Retrieves a paged list of documents based on their archival status.
        /// </summary>
        /// <param name="isArchived">A flag indicating whether to retrieve archived or non-archived documents.</param>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items to display per page.</param>
        /// <returns>
        /// A tuple containing a paged list of documents and pagination information represented by a Pager object.
        /// </returns>
        public (IEnumerable<object>, Pager) GetAllPagedDocuments(bool isArchived, int page, int pageSize)
        {
            IEnumerable<Document> filteredDocuments = _documentRepository.GetAll(isArchived);

            int totalArchivedDocuments = filteredDocuments.Count();
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            var pager = new Pager(totalArchivedDocuments, page, pageSize);

            var pagedDocuments = filteredDocuments
                .Skip(skipCount)
                .Take(pageSize)
                .Select(doc => new
                {
                    doc.DocumentId,
                    doc.File,
                    doc.Date,
                    doc.CustomerId,
                    Type = doc.Type.ToString().Replace("_", " ")
                })
                .ToList();

            return (pagedDocuments.Cast<object>(), pager);
        }

        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public Document GetById(int id)
        {
            return _documentRepository.GetById(id);
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
        public void Put(Document document)
        {
            _documentRepository.Update(document);
        }
    }
}