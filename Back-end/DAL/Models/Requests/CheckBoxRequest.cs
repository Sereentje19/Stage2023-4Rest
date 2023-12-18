namespace DAL.Models.Requests
{
    public class CheckBoxRequest
    {
        public int DocumentId { get; set; }
        public bool IsArchived { get; set; }
    }
}
