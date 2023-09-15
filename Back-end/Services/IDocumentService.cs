using System;
using Back_end.Models;

namespace Back_end.Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetAll();
        Document GetById(int id);
        void Post(Document document);
        void Delete(Document document);
        void Put(Document document);
    }
}