using Back_end.Exceptions;
using Back_end.Models;
using Back_end.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<Document> _dbSet;

        /// <summary>
        /// Initializes a new instance of the DocumentRepository class with the provided NotificationContext.
        /// </summary>
        /// <param name="context">The NotificationContext used for database interactions.</param>
        public DocumentRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Document>();
        }

        public List<Document> GetFilterDocuments(string searchfield, Models.Type? dropBoxType, string overviewType)
        {
            DateTime now = DateTime.Now;
            DateTime sixWeeksFromNow = now.AddDays(42);
            DateTime sixWeeksAgo = now.AddDays(-42);

            var query = from document in _context.Documents
                        join customer in _context.Customers on document.CustomerId equals customer.CustomerId
                        where string.IsNullOrEmpty(searchfield) ||
                            customer.Name.Contains(searchfield) ||
                            customer.Email.Contains(searchfield) ||
                            customer.CustomerId.ToString().Contains(searchfield)
                        where dropBoxType == Models.Type.Not_Selected || document.Type.Equals(dropBoxType)
                        orderby document.Date
                        select new
                        {
                            Document = document
                        };

            if (overviewType == "overview")
            {
                query = query.Where(item => item.Document.Date >= now && item.Document.Date <= sixWeeksFromNow && !item.Document.IsArchived);
            }
            else if (overviewType == "archive")
            {
                query = query.Where(item => item.Document.IsArchived);
            }
            else if (overviewType == "valid")
            {
                query = query.Where(item => item.Document.Date > sixWeeksFromNow && !item.Document.IsArchived);
            }

            var documents = query.Select(item => item.Document).ToList();
            return documents;
        }


        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public DocumentDTO GetById(int id)
        {
            Document doc = _dbSet.Find(id);

            var documentDto = new DocumentDTO
            {
                File = doc.File,
                FileType = doc.FileType,
                Date = doc.Date,
                CustomerId = doc.CustomerId,
                Type = doc.Type,
            };
            return documentDto;
        }


        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="entity">The document entity to be added.</param>
        public void Add(Document entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="entity">The document entity to be updated.</param>
        public void Update(EditDocumentRequestDTO entity)
        {
            var existingDocument = _dbSet.Find(entity.DocumentId);

            if (entity.Type == Models.Type.Not_Selected || string.IsNullOrEmpty(entity.Date.ToString()))
            {
                throw new UpdateDocumentFailedException("Datum of type is leeg.");
            }

            _context.Entry(existingDocument).CurrentValues.SetValues(entity);
            _context.Entry(existingDocument).Property(x => x.File).IsModified = false;
            _context.Entry(existingDocument).Property(x => x.CustomerId).IsModified = false;
            _context.SaveChanges();
        }

        public void UpdateIsArchived(CheckBoxDTO entity)
        {
            var existingDocument = _dbSet.Find(entity.DocumentId);

            if (existingDocument != null)
            {
                existingDocument.IsArchived = entity.IsArchived; 
                _context.SaveChanges(); 
            }
        }
    }
}