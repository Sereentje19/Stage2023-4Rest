using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<Document> getAllDocuments();

        (IEnumerable<object>, int) GetFilteredPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        DocumentDTO GetDocumentById(int id);
        void AddDocument(Document document);
        void UpdateDocument(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO document);
        void DeleteDocument(int id);
    }
}