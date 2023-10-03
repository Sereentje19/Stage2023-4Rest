using System;
using Back_end.Repositories;
using Back_end.Models;

namespace Back_end.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository dr)
        {
            _documentRepository = dr;
        }

        public (IEnumerable<object>, Pager) GetAllPagedDocuments(bool isArchived, int page, int pageSize)
        {
            IEnumerable<Document> filteredDocuments = _documentRepository.GetAll(isArchived);

            int totalArchivedDocuments = filteredDocuments.Count();
            int skipCount = Math.Max(0, (page - 1) * pageSize);
            var pager = new Pager(totalArchivedDocuments, page, pageSize);

            var pagedDocuments = filteredDocuments
                .Skip(skipCount)
                .Take(pageSize)
                .Select(doc => new
                {
                    doc.DocumentId,
                    doc.Image,
                    doc.Date,
                    doc.CustomerId,
                    Type = doc.Type.ToString()
                })
                .ToList();

            return (pagedDocuments.Cast<object>(), pager);
        }


        public Document GetById(int id)
        {
            return _documentRepository.GetById(id);
        }

        public void Post(Document document)
        {
            _documentRepository.Add(document);
        }

        public void Put(Document document)
        {
            _documentRepository.Update(document);
        }
    }
}