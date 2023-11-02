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

        public IEnumerable<Document> getAllDocuments()
        {
            return _dbSet
                .Include(d => d.Customer)
                .ToList();
        }

        public List<Document> GetFilteredDocuments(string searchfield, DocumentType? dropdown, string overviewType)
        {
            try
            {
                IQueryable<Document> query = from document in _context.Documents
                                            .Include(d => d.Customer)
                                             where (string.IsNullOrEmpty(searchfield) ||
                                                 document.Customer.Name.Contains(searchfield) ||
                                                 document.Customer.Email.Contains(searchfield))
                                             && (dropdown == DocumentType.Not_Selected || document.Type == dropdown)
                                             orderby document.Date
                                             select document;

                IQueryable<Document> filteredQuery = ApplyOverviewFilter(query, overviewType);
                var documents = filteredQuery.ToList();
                return documents;
            }
            catch (Exception ex)
            {
                return new List<Document>();
            }
        }

        private IQueryable<Document> ApplyOverviewFilter(IQueryable<Document> query, string overviewType)
        {
            DateTime now = DateTime.Now;
            DateTime sixWeeksFromNow = now.AddDays(42);

            if (query.Count() == 0)
            {
                throw new Exception("Geen document gevonden. Probeer het later onpnieuw.");
            }

            switch (overviewType)
            {
                case "Overzicht":
                    return query.Where(item => item.Date <= sixWeeksFromNow && !item.IsArchived);
                case "Archief":
                    return query.Where(item => item.IsArchived);
                case "Lang geldig":
                    return query.Where(item => item.Date > sixWeeksFromNow && !item.IsArchived);
                default:
                    return query;
            }
        }


        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public DocumentDTO GetDocumentById(int id)
        {
            Document doc = _dbSet
                .Include(d => d.Customer)
                .FirstOrDefault(d => d.DocumentId == id);

            return new DocumentDTO
            {
                File = doc.File,
                FileType = doc.FileType,
                Date = doc.Date,
                Customer = doc.Customer,
                Type = doc.Type
            };
        }


        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public void AddDocument(Document document)
        {
            if (document.Type == DocumentType.Not_Selected)
            {
                throw new DocumentAddException("Selecteer een type.");
            }
            else if (string.IsNullOrWhiteSpace(document.Customer.Name) || string.IsNullOrWhiteSpace(document.Customer.Email))
            {
                throw new CustomerAddException("Klant naam of email is leeg.");
            }

            var existingCustomer = _context.Customers
            .SingleOrDefault(l => l.Name == document.Customer.Name && l.Email == document.Customer.Email);

            if (existingCustomer != null)
            {
                document.Customer = existingCustomer;
            }

            _dbSet.Add(document);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public void UpdateDocument(EditDocumentRequestDTO document)
        {
            var existingDocument = _dbSet
            .Include(d => d.Customer)
            .Where(d => d.DocumentId == document.DocumentId)
            .FirstOrDefault();

            if (existingDocument == null)
            {
                throw new UpdateDocumentFailedException("Geen document gevonden. Probeer het later onpnieuw.");
            }
            else if (document.Type == DocumentType.Not_Selected || string.IsNullOrEmpty(document.Date.ToString()))
            {
                throw new UpdateDocumentFailedException("Datum of type is leeg.");
            }

            existingDocument.Date = document.Date;
            existingDocument.Type = document.Type;

            _context.SaveChanges();
        }

        public void UpdateIsArchived(CheckBoxDTO document)
        {
            var existingDocument = _dbSet.Find(document.DocumentId);

            if (existingDocument == null)
            {
                throw new UpdateDocumentFailedException("Geen document gevonden. Probeer het later onpnieuw.");
            }

            existingDocument.IsArchived = document.IsArchived;
            _context.SaveChanges();
        }

        public void DeleteDocument(int id)
        {
            Document doc =_dbSet.Find(id);

            if (doc == null)
            {
                throw new UpdateDocumentFailedException("Geen document gevonden. Probeer het later onpnieuw.");
            }

            _dbSet.Remove(doc);
            _context.SaveChanges();
        }
    }
}