using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stage4rest2023.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        
        [Column(TypeName = "nvarchar(24)")]
        public ProductType Type { get; set; }
        public string? SerialNumber { get; set; }
    }
}