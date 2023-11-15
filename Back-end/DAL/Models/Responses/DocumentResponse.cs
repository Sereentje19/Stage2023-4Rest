using PL.Models;

namespace PL.Models.Responses
{
    public class DocumentResponse
    {
        public byte[]? File { get; set; }
        public string? FileType { get; set; }
        public DateTime Date { get; set; }
        public Employee Employee { get; set; }
        public DocumentType Type { get; set; }
    }
}
