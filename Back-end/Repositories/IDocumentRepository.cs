using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface IDocumentRepository
    {
        DocumentDTO GetById(int id);
        List<Document> GetFilterDocuments(string searchfield, Models.Type? dropBoxType, string overviewType);
        void Add(Document entity);
        void Update(EditDocumentRequestDTO entity);
        void UpdateIsArchived(CheckBoxDTO entity);
    }
}