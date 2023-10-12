using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
{
    public interface IDocumentService
    {
        (IEnumerable<object>, Pager) GetFilterDocuments(string searchfield, Models.Type? dropBoxType, int page, int pageSize, string overviewType);
        DocumentDTO GetById(int id);
        void Post(Document document);
        void Put(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO entity);
    }
}