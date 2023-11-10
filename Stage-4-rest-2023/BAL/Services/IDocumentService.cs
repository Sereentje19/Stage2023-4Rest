using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Services
{
    public interface IDocumentService
    {
        (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown, int page,
            int pageSize);
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);
        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);
        Task<DocumentDTO> GetDocumentById(int id);
        Task PostDocument(Document document);
        Task PutDocument(EditDocumentRequest document);
        Task UpdateIsArchived(CheckBoxRequest entity);
        Task DeleteDocument(int id);
    }
}