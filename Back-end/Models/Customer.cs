using System.ComponentModel.DataAnnotations;

namespace Back_end.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}