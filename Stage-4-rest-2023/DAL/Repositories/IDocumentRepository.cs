using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Repositories
{
    public interface IDocumentRepository
    {

        (IEnumerable<object>, int) GetPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        Task<DocumentResponse> GetDocumentById(int id);
        Task AddDocument(Document document);
        Task UpdateDocument(EditDocumentRequest document);
        Task UpdateIsArchived(CheckBoxRequest document);
        Task DeleteDocument(int id);
    }
}