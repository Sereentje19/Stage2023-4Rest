namespace DAL.Models.Requests;

public class EmployeeRequestDto
{
    public int EmployeeId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public bool IsArchived { get; set; }
}