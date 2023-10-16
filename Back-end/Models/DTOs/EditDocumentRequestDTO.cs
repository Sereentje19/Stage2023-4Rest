namespace Back_end.Models.DTOs
{
    public class EditDocumentRequestDTO
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public Type Type { get; set; }
        public int CustomerId { get; set; }
    }
}