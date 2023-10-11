namespace Back_end.Models.DTOs
{
    public class OverviewResponseDTO
    {
        public int DocumentId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Type Type { get; set; }
    }
}