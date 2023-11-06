using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface IDocumentService
    {
        IEnumerable<Document> GetAllDocuments();
        (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown, int page,
            int pageSize);
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);
        public (IEnumerable<object>, Pager) GetFilteredPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);
        object GetDocumentById(int id);
        void PostDocument(Document document);
        void PutDocument(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO entity);
        void DeleteDocument(int id);
    }
}