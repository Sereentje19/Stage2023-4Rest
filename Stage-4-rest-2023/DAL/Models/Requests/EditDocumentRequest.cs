namespace Stage4rest2023.Models.DTOs
{
    public class EditDocumentRequest
    {
        public int DocumentId { get; }
        public DateTime Date { get; }
        public DocumentType Type { get; }
    }
}