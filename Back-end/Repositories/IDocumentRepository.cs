using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<Document> GetAll();
        List<Document> GetFilterDocuments(string searchfield, DocumentType? dropBoxType, string overviewType);
        DocumentDTO GetById(int id);
        IEnumerable<Document> GetByCustomerId(int customerId);
        void Add(Document entity);
        void Update(EditDocumentRequestDTO entity);
        void UpdateIsArchived(CheckBoxDTO entity);
        void UpdateCustomerId(int customerId, int documentId);
    }
}