namespace DAL.Models.Requests
{
    public class CheckBoxRequestDto
    {
        public int DocumentId { get; set; }
        public bool IsArchived { get; set; }
    }
}
