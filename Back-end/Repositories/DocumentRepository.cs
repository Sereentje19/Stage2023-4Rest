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

        public IEnumerable<Document> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(Document entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(Document entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Document entity)
        {
            _dbSet.Remove(entity);
        }
    }
}