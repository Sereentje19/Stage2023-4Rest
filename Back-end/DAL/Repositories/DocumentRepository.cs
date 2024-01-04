using System.Collections;
using System.Linq.Expressions;
using DAL.Data;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Document> _dbSet;
        private const int SixWeeksFromNow = 42;
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
        private IQueryable<DocumentOverviewResponseDto> QueryGetDocuments(string searchfield, string dropdown)
        {
            return _context.Documents
                .Include(d => d.Employee)
                .Where(document => (string.IsNullOrEmpty(searchfield) ||
                                    document.Employee.Name.Contains(searchfield) ||
                                    document.Employee.Email.Contains(searchfield))
                                   && (dropdown == "0" || document.Type.Id.ToString() == dropdown))
                .OrderBy(document => document.Date)
                .Select(doc => new DocumentOverviewResponseDto
                {
                    DocumentId = doc.DocumentId,
                    EmployeeId = doc.Employee.EmployeeId,
                    Date = doc.Date,
                    EmployeeName = doc.Employee.Name,
                    IsArchived = doc.IsArchived,
                    Type = doc.Type,
                });
        }
        
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
            string dropdown, int page, int pageSize, Expression<Func<DocumentOverviewResponseDto, bool>> filter)
        {
            int skipCount = (page - 1) * pageSize;
            IQueryable<DocumentOverviewResponseDto> query = QueryGetDocuments(searchfield, dropdown).Where(filter);
            int numberOfCustomers = query.Count();
            
            IEnumerable<DocumentOverviewResponseDto> documentList = query
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
            DateTime sixWeeksFromNow = DateTime.Now.AddDays(SixWeeksFromNow);
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
            DateTime sixWeeksFromNow = DateTime.Now.AddDays(SixWeeksFromNow);
            return GetPagedDocumentsInternal(searchfield, dropdown, page, pageSize,
                item => item.Date > sixWeeksFromNow && !item.IsArchived);
        }


        /// <summary>
        /// Retrieves a list of all document types asynchronously.
        /// </summary>
        /// <returns>An IEnumerable of DocumentType representing the list of document types.</returns>
        public async Task<IEnumerable<DocumentType>> GetDocumentTypesAsync()
        {
            return await _context.DocumentTypes.ToListAsync();
        }


        /// <summary>
        /// Retrieves a document by its unique identifier (ID).
        /// </summary>
        /// <param name="id">The unique identifier of the document to retrieve.</param>
        /// <returns>The document with the specified ID if found; otherwise, returns null.</returns>
        public Task<DocumentResponseDto> GetDocumentByIdAsync(int id)
        {
            return _dbSet
                .Where(d => d.DocumentId == id)
                .Include(d => d.Employee)
                .Select(doc => new DocumentResponseDto
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
        public async Task CreateDocumentAsync(Document document)
        {
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
        public async Task UpdateDocumentAsync(EditDocumentRequestDto document)
        {
            Document existingDocument = await _dbSet
                .Include(d => d.Employee)
                .Where(d => d.DocumentId == document.DocumentId)
                .FirstOrDefaultAsync();

            existingDocument.Date = document.Date;
            existingDocument.Type = document.Type;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the archival status of a document based on the provided information.
        /// </summary>
        /// <param name="document">The CheckBoxDTO containing document information.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateIsArchivedAsync(CheckBoxRequestDto document)
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
        public async Task DeleteDocumentAsync(int id)
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