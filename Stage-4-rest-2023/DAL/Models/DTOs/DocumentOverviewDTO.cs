namespace Stage4rest2023.Models.DTOs;

public class DocumentOverviewDTO
{
    public int DocumentId { get; set; }
    public DateTime Date { get; set; }
    public string CustomerName { get; set; }    
    public int CustomerId { get; set; }
    public string Type { get; set; }
    public bool IsArchived { get; set; }
}