using PL.Models;

namespace PL.Models.Requests
{
    public class EditDocumentRequest
    {
        public int DocumentId { get; }
        public DateTime Date { get; }
        public DocumentType Type { get; }
    }
}
