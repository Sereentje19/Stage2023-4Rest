using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public byte[] File { get; set; }
        [StringLength(250, ErrorMessage = "Er is iets fout gegaan bij het opslaan van de file.")]
        public string FileType { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [ForeignKey("TypeId")]
        public DocumentType Type { get; set; }
        public bool IsArchived { get; set; }
    }
}
