using Back_end.Models;
using Back_end.Models.DTOs;

namespace Back_end.Repositories
{
    public interface IDocumentRepository
    {
        DocumentDTO GetById(int id);
        IEnumerable<OverviewResponseDTO> GetAll(bool isArchived);
        void Add(Document entity);
        void Update(EditDocumentRequestDTO entity);
    }
}