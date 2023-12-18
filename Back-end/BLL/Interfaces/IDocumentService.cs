using DAL.Models;
using DAL.Models.Requests;
using DAL.Models.Responses;

namespace BLL.Interfaces
{
    public interface IDocumentService
    {
        (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, string dropdown, int page,
            int pageSize);
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);
        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        Task<IEnumerable<DocumentType>> GetDocumentTypesAsync();
        Task<DocumentResponse> GetDocumentByIdAsync(int id);
        Task CreateDocumentAsync(Document document);
        Task UpdateDocumentAsync(EditDocumentRequest document);
        Task UpdateIsArchivedAsync(CheckBoxRequest entity);
        Task DeleteDocumentAsync(int id);
    }
}
