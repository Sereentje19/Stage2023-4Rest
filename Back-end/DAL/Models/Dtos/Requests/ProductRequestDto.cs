namespace DAL.Models.Requests;

public class ProductRequestDto
{
    public int ProductId { get; set; }
    public byte[] File { get; set; }
    public string FileType { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime PurchaseDate { get; set; }
    public ProductType Type { get; set; }
    public string SerialNumber { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime TimeDeleted { get; set; }
}