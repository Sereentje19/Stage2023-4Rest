using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface IDocumentRepository
    {
        DocumentDTO GetById(int id);
        IEnumerable<OverviewResponseDTO> GetAll(bool isArchived);
        List<Document> GetFilterDocuments(string searchfield, Models.Type? dropBoxType);
        void Add(Document entity);
        void Update(EditDocumentRequestDTO entity);
    }
}