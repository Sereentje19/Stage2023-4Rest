using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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