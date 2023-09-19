using System;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface IDocumentRepository
    {
        Document GetById(int id);
        IEnumerable<Document> GetAll();
        void Add(Document entity);
        void Update(Document entity);
        void Delete(Document entity);
    }
}