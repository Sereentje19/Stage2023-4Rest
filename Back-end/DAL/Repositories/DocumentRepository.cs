using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models.Requests;
using PL.Models.Responses;
using PL.Models;

namespace DAL.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Document> _dbSet;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Document>();
        }

        /// <summary>
        /// Queries and retrieves a collection of document overviews based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <returns>An IQueryable of DocumentOverviewDTO representing the document overviews.</returns>
        private IQueryable<DocumentOverviewResponse> QueryGetDocuments(string searchfield, string dropdown)
        {
            return _context.Documents
                .Include(d => d.Employee)
                .Where(document => (string.IsNullOrEmpty(searchfield) ||
                                    document.Employee.Name.Contains(searchfield) ||
                                    document.Employee.Email.Contains(searchfield))
                                   && (dropdown == "0" || document.Type.Id.ToString() == dropdown))
                .OrderBy(document => document.Date)
                .Select(doc => new DocumentOverviewResponse
                {
                    DocumentId = doc.DocumentId,
                    EmployeeId = doc.Employee.EmployeeId,
                    Date = doc.Date,
                    EmployeeName = doc.Employee.Name,
                    IsArchived = doc.IsArchived,
                    Type = doc.Type,
                });
        }

        //BRFerFC5AFJwE5A7S3alYFVo9olS0aveXC6IgwYqMc0=
        //LueZDV6TpgrQ3fkXJrWg20cuqAhvQ5VKPqpEkbbBBdI=
        
        /// <summary>
        /// Retrieves a paged list of document overviews based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of document overviews per page.</param>
        /// <param name="filter">Additional filtering function for document overviews.</param>
        /// <returns>A tuple containing a collection of document overviews and the total number of document overviews.</returns>
        private (IEnumerable<object>, int) GetPagedDocumentsInternal(string searchfield,
            string dropdown, int page, int pageSize, Expression<Func<DocumentOverviewResponse, bool>> filter)
        {
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            IQueryable<DocumentOverviewResponse> query = QueryGetDocuments(searchfield, dropdown).Where(filter);
            int numberOfCustomers = query.Count();

            IEnumerable<DocumentOverviewResponse> documentList = query
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            return (documentList, numberOfCustomers);
        }

        /// <summary>
        /// Retrieves a paged list of documents based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>A tuple containing a collection of documents and the total number of documents.</returns>
        public (IEnumerable<object>, int) GetPagedDocuments(string searchfield, string dropdown, int page,
            int pageSize)
        {
            DateTime sixWeeksFromNow = DateTime.Now.AddDays(42);
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize,
                item => item.Date <= sixWeeksFromNow && !item.IsArchived);
        }

        /// <summary>
        /// Retrieves a paged list of archived documents based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>A tuple containing a collection of archived documents and the total number of documents.</returns>
        public (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize)
        {
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize, item => item.IsArchived);
        }

        /// <summary>
        /// Retrieves a paged list of long-valid documents based on specified criteria.
        /// </summary>
        /// <param name="searchfield">The search criteria for customer names or emails.</param>
        /// <param name="dropdown">The document type filter.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of documents per page.</param>
        /// <returns>A tuple containing a collection of long-valid documents and the total number of documents.</returns>
        public (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize)
        {
            DateTime sixWeeksFromNow = DateTime.Now.AddDays(42);
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize,
                item => item.Date > sixWeeksFromNow && !item.IsArchived);
        }


        public async Task<IEnumerable<DocumentType>> GetDocumentTypes()
        {
            return await _context.DocumentTypes.ToListAsync();
        }

        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public async Task<DocumentResponse> GetDocumentById(int id)
        {
            return await _dbSet
                .Where(d => d.DocumentId == id)
                .Include(d => d.Employee)
                .Select(doc => new DocumentResponse
                {
                    File = doc.File,
                    FileType = doc.FileType,
                    Date = doc.Date,
                    Employee = doc.Employee,
                    Type = doc.Type
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds a new document to the repository.
        /// </summary>
        /// <param name="document">The document entity to be added.</param>
        public async Task AddDocument(Document document)
        {
            if (document.Type.Name == "0")
            {
                throw new InputValidationException("Selecteer een type.");
            }

            if (string.IsNullOrWhiteSpace(document.Employee.Email) || !document.Employee.Email.Contains("@"))
            {
                throw new InputValidationException("Geen geldige email.");
            }

            if (string.IsNullOrWhiteSpace(document.Employee.Name))
            {
                throw new InputValidationException("Klant naam is leeg.");
            }

            if (document.Date < DateTime.Today)
            {
                throw new InputValidationException("Datum is incorrect, de datum moet in de toekomst zijn.");
            }

            Employee existingEmployee = await _context.Employees
                .SingleOrDefaultAsync(l => l.Email == document.Employee.Email);

            DocumentType type = await _context.DocumentTypes
                .SingleOrDefaultAsync(t => t.Name == document.Type.Name);

            if (type != null)
            {
                document.Type = type;
            }

            if (existingEmployee != null)
            {
                document.Employee = existingEmployee;
            }

            await _dbSet.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing document in the repository.
        /// </summary>
        /// <param name="document">The document entity to be updated.</param>
        public async Task UpdateDocument(EditDocumentRequest document)
        {
            Document existingDocument = await _dbSet
                .Include(d => d.Employee)
                .Where(d => d.DocumentId == document.DocumentId)
                .FirstOrDefaultAsync();

            if (document.Type.Name == "0")
            {
                throw new InputValidationException("Selecteer een type.");
            }

            
            // if (document.Date < DateTime.Today)
            // {
            //     throw new InputValidationException("Datum is incorrect, de datum moet in de toekomst zijn.");
            // }

            existingDocument.Date = document.Date;
            existingDocument.Type = document.Type;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the archival status of a document based on the provided information.
        /// </summary>
        /// <param name="document">The CheckBoxDTO containing document information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateIsArchived(CheckBoxRequest document)
        {
            Document existingDocument = await _dbSet.FindAsync(document.DocumentId);

            if (existingDocument == null)
            {
                throw new NotFoundException("Geen document gevonden");
            }

            existingDocument.IsArchived = document.IsArchived;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a document based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the document to be deleted.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task DeleteDocument(int id)
        {
            Document doc = await _dbSet.FindAsync(id);

            if (doc == null)
            {
                throw new NotFoundException("Geen document gevonden");
            }

            _dbSet.Remove(doc);
            await _context.SaveChangesAsync();
        }
    }
}