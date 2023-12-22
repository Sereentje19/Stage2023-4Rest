namespace DAL.Models.Dtos.Requests
{
    public class CheckBoxRequestDto
    {
        public int DocumentId { get; set; }
        public bool IsArchived { get; set; }
    }
}
