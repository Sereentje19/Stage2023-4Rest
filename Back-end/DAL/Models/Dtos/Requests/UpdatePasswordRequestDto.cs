namespace DAL.Models.Dtos.Requests;

public class UpdatePasswordRequestDto
{
    
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string Password1 { get; set; }
    public string Password2 { get; set; }
    public string Password3 { get; set; }
    
}