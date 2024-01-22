using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "Het type mag niet meer dan {1} tekens bevatten.")]
        public string Name { get; set; }
    }
}