using PL.Models.Requests;
using PL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PL.Models;

namespace DAL.Repositories
{
    public interface IDocumentRepository
    {

        (IEnumerable<object>, int) GetPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        Task<IEnumerable<DocumentType>> GetDocumentTypes();
        Task<DocumentResponse> GetDocumentById(int id);
        Task AddDocument(Document document);
        Task UpdateDocument(EditDocumentRequest document);
        Task UpdateIsArchived(CheckBoxRequest document);
        Task DeleteDocument(int id);
    }
}
