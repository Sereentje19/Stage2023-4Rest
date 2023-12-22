namespace DAL.Models.Dtos.Responses
{
    public class DocumentOverviewResponseDto
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public DocumentType Type { get; set; }
        public bool IsArchived { get; set; }
    }
}
