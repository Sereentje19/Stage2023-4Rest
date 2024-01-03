using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class DocumentType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}