namespace PL.Models.Responses
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ProductType Type { get; set; }
        public string SerialNumber { get; set; }
    }
}
