using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
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