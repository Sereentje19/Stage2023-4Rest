using System;
using Back_end.Models;

namespace Back_end.Services
{
    public interface IDocumentService
    {
        (IEnumerable<object>, Pager) GetAllPagedDocuments(bool isArchived, int page, int pageSize);
        Document GetById(int id);
        void Post(Document document);
        void Put(Document document);
    }
}