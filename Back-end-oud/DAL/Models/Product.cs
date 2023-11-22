using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
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
