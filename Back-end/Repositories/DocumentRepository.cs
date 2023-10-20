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

        public async Task<IEnumerable<Document>> getAll()
        {
            List<Document> documents = await _context.Documents
                .Include(d => d.Customer)
                .ToListAsync();

            var documentDTOs = documents.Select(doc => new CustomerDocumentDTO
            {
                DocumentId = doc.DocumentId,
                CustomerId = doc.Customer.CustomerId,
            });

            return documents;
        }

        public List<Document> GetFilterDocuments(string searchfield, DocumentType? dropBoxType, string overviewType)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime sixWeeksFromNow = now.AddDays(42);
                DateTime sixWeeksAgo = now.AddDays(-42);

                var query = from document in _context.Documents
                .Include(d => d.Customer)
                            where (string.IsNullOrEmpty(searchfield) ||
                                   document.Customer.Name.Contains(searchfield) ||
                                   document.Customer.Email.Contains(searchfield) ||
                                   document.Customer.CustomerId.ToString().Contains(searchfield))
                            && (dropBoxType == DocumentType.Not_Selected || document.Type == dropBoxType)
                            orderby document.Date
                            select new
                            {
                                Document = document
                            };

                if (query.Count() == 0)
                {
                    throw new Exception("No documents found.");
                }
                else if (overviewType == "Overzicht")
                {
                    query = query.Where(item => item.Document.Date <= sixWeeksFromNow && !item.Document.IsArchived);
                }
                else if (overviewType == "Archief")
                {
                    query = query.Where(item => item.Document.IsArchived);
                }
                else if (overviewType == "Lang geldig")
                {
                    query = query.Where(item => item.Document.Date > sixWeeksFromNow && !item.Document.IsArchived);
                }

                var documents = query.Select(item => item.Document).ToList();
                return documents;
            }
            catch (Exception ex)
            {
                return new List<Document>();
            }
        }


        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public DocumentDTO GetById(int id)
        {
            Document doc = _dbSet
            .Include(d => d.Customer)
            .FirstOrDefault(d => d.DocumentId == id);

            var documentDto = new DocumentDTO
            {
                File = doc.File,
                FileType = doc.FileType,
                Date = doc.Date,
                customer = doc.Customer,
                Type = doc.Type,
            };

            return documentDto;
        }

        public IEnumerable<Document> GetByCustomerId(int customerId)
        {
            var documents = _dbSet
            .Include(d => d.Customer)
            .Where(d => d.Customer.CustomerId == customerId)
            .ToList();

            return documents;
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
            var existingDocument = _dbSet
            .Include(d => d.Customer)
            .Where(d => d.DocumentId == entity.DocumentId)
            .FirstOrDefault();

            existingDocument.Date = entity.Date;
            existingDocument.Type = entity.Type;

            if (existingDocument == null)
            {
                throw new UpdateDocumentFailedException("Document not found.");
            }
            else if (entity.Type == DocumentType.Not_Selected || string.IsNullOrEmpty(entity.Date.ToString()))
            {
                throw new UpdateDocumentFailedException("Datum of type is leeg.");
            }

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

        public void UpdateCustomerId(int customerId, int documentId)
        {
            Document existingDocument = _dbSet
            .Include(d => d.Customer)
            .FirstOrDefault(d => d.DocumentId == documentId);

            if (existingDocument != null)
            {
                existingDocument.Customer.CustomerId = customerId;
                _context.SaveChanges();
            }
        }
    }
}