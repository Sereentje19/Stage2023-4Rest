namespace DAL.Models.Requests
{
    public class EditDocumentRequestDto
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public DocumentType Type { get; set; }
    }
}
