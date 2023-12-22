namespace DAL.Models.Dtos.Requests;

public class LoanHistoryRequestDto
{
    public int LoanHistoryId { get; set; }
    public Employee Employee { get; set; }
    public Product Product { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}