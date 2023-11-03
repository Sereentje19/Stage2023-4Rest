namespace Stage4rest2023.Models.DTOs
{
    public class DocumentDTO
    {
        public byte[]? File { get; set; }
        public string? FileType { get; set; }
        public DateTime Date { get; set; }
        public Customer Customer { get; set; }
        public DocumentType Type { get; set; }
    }
}