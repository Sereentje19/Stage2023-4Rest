using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetAllDocuments();
        (IEnumerable<object>, Pager) GetFilteredDocuments(string searchfield, DocumentType? dropBoxType, int page, int pageSize, string overviewType);
        object GetDocumentById(int id);
        void PostDocument(Document document);
        void PutDocument(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO entity);
        void DeleteDocument(int id);
    }
}