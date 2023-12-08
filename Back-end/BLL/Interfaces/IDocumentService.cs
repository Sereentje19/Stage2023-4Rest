using Azure;
using PL.Models.Requests;
using PL.Models.Responses;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IDocumentService
    {
        (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, string dropdown, int page,
            int pageSize);
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);
        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, string dropdown,
            int page, int pageSize);

        Task<IEnumerable<DocumentType>> GetDocumentTypes();
        Task<DocumentResponse> GetDocumentById(int id);
        Task PostDocument(Document document);
        Task PutDocument(EditDocumentRequest document);
        Task UpdateIsArchived(CheckBoxRequest entity);
        Task DeleteDocument(int id);
    }
}
