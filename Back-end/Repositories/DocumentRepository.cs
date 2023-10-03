using System;
using Back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_end.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly NotificationContext _context;
        private readonly DbSet<Document> _dbSet;

        public DocumentRepository(NotificationContext context)
        {
            _context = context;
            _dbSet = _context.Set<Document>();
        }

        public Document GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<Document> GetAll(bool isArchived)
        {
            IEnumerable<Document> allDocuments = _dbSet.ToList();
            IEnumerable<Document> filteredDocuments;
            DateTime currentDate = DateTime.Now;

            if (isArchived)
            {
                filteredDocuments = allDocuments.Where(doc => doc.Date < currentDate)
                .OrderByDescending(doc => doc.Date);
            }
            else
            {
                filteredDocuments = allDocuments.Where(doc => doc.Date > currentDate)
                .OrderBy(doc => doc.Date);
            }

            return filteredDocuments;
        }

        public void Add(Document entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Document entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}