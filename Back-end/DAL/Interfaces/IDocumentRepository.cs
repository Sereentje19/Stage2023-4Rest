using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;

namespace DAL.Interfaces
{
    public interface IDocumentRepository
    {

        (IEnumerable<object>, int) GetPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        Task<IEnumerable<DocumentType>> GetDocumentTypesAsync();
        Task<DocumentResponseDto> GetDocumentByIdAsync(int id);
        Task CreateDocumentAsync(Document document);
        Task UpdateDocumentAsync(EditDocumentRequestDto document);
        Task UpdateIsArchivedAsync(CheckBoxRequestDto document);
        Task DeleteDocumentAsync(int id);
    }
}
