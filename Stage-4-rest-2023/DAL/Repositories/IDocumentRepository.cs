using Stage4rest2023.Models;
using Stage4rest2023.Models.DTOs;

namespace Stage4rest2023.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<Document> getAllDocuments();
        List<Document> GetFilteredDocuments(string searchfield, DocumentType? dropdown, string overviewType);
        DocumentDTO GetDocumentById(int id);
        void AddDocument(Document document);
        void UpdateDocument(EditDocumentRequestDTO document);
        void UpdateIsArchived(CheckBoxDTO document);
        void DeleteDocument(int id);
    }
}