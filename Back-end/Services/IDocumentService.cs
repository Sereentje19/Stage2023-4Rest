using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetAll();
        (IEnumerable<object>, Pager) GetFilterDocuments(string searchfield, Models.Type? dropBoxType, int page, int pageSize, string overviewType);
        DocumentDTO GetById(int id);
        IEnumerable<Document> GetByCustomerId(int customerId);
        void Post(Document document);
        void Put(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO entity);
    }
}