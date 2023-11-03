namespace Stage4rest2023.Models.DTOs
{
    public class EditDocumentRequestDTO
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public DocumentType Type { get; set; }
        public int CustomerId { get; set; }
    }
}