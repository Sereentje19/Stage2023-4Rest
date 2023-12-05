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
        (IEnumerable<object>, Pager) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown, int page,
            int pageSize);
        public (IEnumerable<object>, Pager) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);
        public (IEnumerable<object>, Pager) GetPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        List<string> GetDocumentTypeStrings();
        Task<DocumentResponse> GetDocumentById(int id);
        Task PostDocument(Document document);
        Task PutDocument(EditDocumentRequest document);
        Task UpdateIsArchived(CheckBoxRequest entity);
        Task DeleteDocument(int id);
    }
}
