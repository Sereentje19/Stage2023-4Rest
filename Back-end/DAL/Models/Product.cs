using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public byte[] File { get; set; }
        [StringLength(250, ErrorMessage = "Er is iets fout gegaan bij het opslaan van de file.")]
        public string FileType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        [ForeignKey("TypeId")]
        public ProductType Type { get; set; }
        
        [StringLength(100, ErrorMessage = "Het serienummer mag niet meer dan {1} tekens bevatten.")]
        public string SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeDeleted { get; set; }
        public ICollection<LoanHistory> LoanHistory { get; set; }
    }
}
