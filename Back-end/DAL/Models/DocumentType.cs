using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class DocumentType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}