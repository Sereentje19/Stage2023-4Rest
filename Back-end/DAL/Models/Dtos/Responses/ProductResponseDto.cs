namespace DAL.Models.Dtos.Responses
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ProductType Type { get; set; }
        public string SerialNumber { get; set; }
    }
}
