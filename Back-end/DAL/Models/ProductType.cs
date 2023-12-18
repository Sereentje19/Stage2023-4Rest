using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}