using Stage4rest2023.Exceptions;
using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Stage4rest2023.Repositories
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
        
        public IQueryable<DocumentOverviewDTO> QueryGetDocuments(string searchfield, DocumentType? dropdown, Func<DocumentOverviewDTO, bool> filter)
        {
            return _context.Documents
                .Include(d => d.Customer)
                .Where(document => (string.IsNullOrEmpty(searchfield) ||
                                    document.Customer.Name.Contains(searchfield) ||
                                    document.Customer.Email.Contains(searchfield))
                                   && (dropdown == DocumentType.Not_Selected || document.Type == dropdown))
                .OrderBy(document => document.Date)
                .Select(doc => new DocumentOverviewDTO
                {
                    DocumentId = doc.DocumentId,
                    CustomerId = doc.Customer.CustomerId,
                    Date = doc.Date,
                    CustomerName = doc.Customer.Name,
                    IsArchived = doc.IsArchived,
                    Type = doc.Type.ToString().Replace("_", " "),
                });
        }

        private (IEnumerable<DocumentOverviewDTO>, int) GetPagedDocumentsInternal(string searchfield, DocumentType? dropdown, int page, int pageSize, Func<DocumentOverviewDTO, bool> filter)
        {
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            IQueryable<DocumentOverviewDTO> query = QueryGetDocuments(searchfield, dropdown, filter);
            int numberOfCustomers = query.Where(filter).Count();
            
            IEnumerable<DocumentOverviewDTO> documentList = query
                .Where(filter)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (documentList, numberOfCustomers);
        }
        
        public (IEnumerable<object>, int) GetPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            DateTime sixWeeksFromNow = DateTime.Now.AddDays(42);
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize, item => item.Date <= sixWeeksFromNow && !item.IsArchived);
        }

        public (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize, item => item.IsArchived);
        }

        public (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown, int page, int pageSize)
        {
            DateTime sixWeeksFromNow = DateTime.Now.AddDays(42);
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize, item => item.Date > sixWeeksFromNow && !item.IsArchived);
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
            try
            {
                if (document.Type == DocumentType.Not_Selected)
                {
                    throw new InputValidationException("Selecteer een type.");
                }

                if (string.IsNullOrWhiteSpace(document.Customer.Name) ||
                    string.IsNullOrWhiteSpace(document.Customer.Email))
                {
                    throw new InputValidationException("Klant naam of email is leeg.");
                }

                Customer existingCustomer = _context.Customers
                    .SingleOrDefault(l => l.Name == document.Customer.Name && l.Email == document.Customer.Email);

                if (existingCustomer != null)
                {
                    document.Customer = existingCustomer;
                }

                _dbSet.Add(document);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public void UpdateDocument(EditDocumentRequestDTO document)
        {
            try
            {
                Document existingDocument = _dbSet
                    .Include(d => d.Customer)
                    .Where(d => d.DocumentId == document.DocumentId)
                    .FirstOrDefault();

                if (existingDocument == null)
                {
                    throw new ItemNotFoundException("Geen document gevonden. Probeer het later onpnieuw.");
                }

                if (document.Type == DocumentType.Not_Selected || string.IsNullOrEmpty(document.Date.ToString()))
                {
                    throw new InputValidationException("Datum of type is leeg.");
                }

                existingDocument.Date = document.Date;
                existingDocument.Type = document.Type;

                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        public void UpdateIsArchived(CheckBoxDTO document)
        {
            try
            {
                Document existingDocument = _dbSet.Find(document.DocumentId);

                if (existingDocument == null)
                {
                    throw new ItemNotFoundException("Geen document gevonden. Probeer het later onpnieuw.");
                }

                existingDocument.IsArchived = document.IsArchived;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }

        public void DeleteDocument(int id)
        {
            try
            {
                Document doc = _dbSet.Find(id);

                if (doc == null)
                {
                    throw new ItemNotFoundException("Geen document gevonden. Probeer het later onpnieuw.");
                }

                _dbSet.Remove(doc);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(
                    "Er is een conflict opgetreden bij het bijwerken van de gegevens.");
            }
        }
    }
}