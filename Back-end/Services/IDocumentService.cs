using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetAll();
        (IEnumerable<object>, Pager) GetFilterDocuments(string searchfield, DocumentType? dropBoxType, int page, int pageSize, string overviewType);
        object GetById(int id);
        IEnumerable<Document> GetByCustomerId(int customerId);
        void Post(Document document);
        void Put(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO entity);
        void UpdateCustomerId(int customerId, int documentId);
    }
}