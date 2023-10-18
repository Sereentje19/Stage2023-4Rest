namespace Back_end.Models.DTOs
{
    public class DocumentDTO
    {
        public byte[]? File { get; set; }
        public string? FileType { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public DocumentType Type { get; set; }
    }
}