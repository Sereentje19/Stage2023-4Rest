using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}