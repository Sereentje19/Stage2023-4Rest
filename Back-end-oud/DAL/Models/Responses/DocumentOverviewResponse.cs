namespace PL.Models.Responses
{
    public class DocumentOverviewResponse
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public bool IsArchived { get; set; }
    }
}
