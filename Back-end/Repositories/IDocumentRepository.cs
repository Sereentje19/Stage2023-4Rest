using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<Document> getAll();
        List<Document> GetFilteredDocuments(string searchfield, DocumentType? dropdown, string overviewType);
        DocumentDTO GetById(int id);
        IEnumerable<Document> GetByCustomerId(int customerId);
        void Add(Document document);
        void Update(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO document);
        void UpdateCustomerId(int customerId, int documentId);
        void delete(int id);
    }
}