namespace Back_end.Models.DTOs
{
    public class EditDocumentRequestDTO
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public DocumentType Type { get; set; }
        public int CustomerId { get; set; }
    }
}