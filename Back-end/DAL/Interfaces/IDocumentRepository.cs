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

        (IEnumerable<object>, int) GetPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetArchivedPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        (IEnumerable<object>, int) GetLongValidPagedDocuments(string searchfield, DocumentType? dropdown,
            int page, int pageSize);

        List<string> GetDocumentTypeStrings();
        Task<DocumentResponse> GetDocumentById(int id);
        Task AddDocument(Document document);
        Task UpdateDocument(EditDocumentRequest document);
        Task UpdateIsArchived(CheckBoxRequest document);
        Task DeleteDocument(int id);
    }
}
