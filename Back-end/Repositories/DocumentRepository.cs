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
        /// Retrieves a collection of documents based on their archival status.
        /// </summary>
        /// <param name="isArchived">A flag indicating whether to retrieve archived or non-archived documents.</param>
        /// <returns>A collection of documents matching the specified archival status.</returns>
        public IEnumerable<OverviewResponseDTO> GetAll(bool isArchived)
        {
            DateTime currentDate = DateTime.Now;

            var filteredDocuments = isArchived
                ? _dbSet.Where(doc => doc.Date < currentDate).OrderByDescending(doc => doc.Date)
                : _dbSet.Where(doc => doc.Date > currentDate).OrderBy(doc => doc.Date);

            var overviewList = filteredDocuments.Select(doc => new OverviewResponseDTO
            {
                DocumentId = doc.DocumentId,
                Date = doc.Date,
                CustomerId = doc.CustomerId,
                Type = doc.Type
            }).ToList();

            return overviewList;
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
                throw new Exception("Datum of type is leeg.");
            }

            _context.Entry(existingDocument).CurrentValues.SetValues(entity);
            _context.Entry(existingDocument).Property(x => x.File).IsModified = false;
            _context.Entry(existingDocument).Property(x => x.CustomerId).IsModified = false;
            _context.SaveChanges();
        }


    }
}