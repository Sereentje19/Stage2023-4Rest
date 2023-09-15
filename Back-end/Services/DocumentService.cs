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

        public IEnumerable<Document> GetAll()
        {
            return _documentRepository.GetAll();
        }

        public Document GetById(int id)
        {
            return _documentRepository.GetById(id);
        }

        public void Post(Document document)
        {
            _documentRepository.Add(document);
        }

        public void Delete(Document document)
        {
            _documentRepository.Delete(document);
        }

        public void Put(Document document)
        {
            _documentRepository.Update(document);
        }
    }
}