using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Services
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