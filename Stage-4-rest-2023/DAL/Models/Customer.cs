using System.ComponentModel.DataAnnotations;

namespace Stage4rest2023.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}