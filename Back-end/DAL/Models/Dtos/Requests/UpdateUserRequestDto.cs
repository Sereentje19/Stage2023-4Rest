namespace DAL.Models.Dtos.Requests;

public class UpdateUserRequestDto
{
    public int UserId { get; set; }
    public string Email1 { get; set; }
    public string Email2 { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool UpdateName { get; set; }
}