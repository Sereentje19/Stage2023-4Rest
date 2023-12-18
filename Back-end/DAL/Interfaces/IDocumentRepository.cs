using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

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
        Task<DocumentResponse> GetDocumentByIdAsync(int id);
        Task CreateDocumentAsync(Document document);
        Task UpdateDocumentAsync(EditDocumentRequest document);
        Task UpdateIsArchivedAsync(CheckBoxRequest document);
        Task DeleteDocumentAsync(int id);
    }
}
