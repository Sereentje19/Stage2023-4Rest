namespace DAL.Models.Requests;

public class PasswordChangeRequest
{
    
    public string? Email { get; set; }
    public string? Password1 { get; set; }
    public string? Password2 { get; set; }
    public string? Code { get; set; }
}