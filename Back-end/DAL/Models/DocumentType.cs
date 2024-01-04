using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class DocumentType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200, ErrorMessage = "De naam mag niet meer dan {1} tekens bevatten.")]
        public string Name { get; set; }
    }
}