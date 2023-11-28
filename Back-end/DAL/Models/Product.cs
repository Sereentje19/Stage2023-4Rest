using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public DateTime ExpirationDate { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]

        [Column(TypeName = "nvarchar(24)")]
        public ProductType Type { get; set; }
        [Required]
        public string? SerialNumber { get; set; }
    }
}
