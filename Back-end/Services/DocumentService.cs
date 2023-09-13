using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _documentRepository;

        public DocumentService(IRepository<Document> dr)
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