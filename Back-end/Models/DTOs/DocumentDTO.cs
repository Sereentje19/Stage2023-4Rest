namespace Back_end.Models.DTOs
{
    public class DocumentDTO
    {
        public byte[]? File { get; set; }
        public string? FileType { get; set; }
        public DateTime Date { get; set; }
        public Customer customer { get; set; }
        public DocumentType Type { get; set; }
    }
}