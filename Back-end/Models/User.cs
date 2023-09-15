using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Back_end.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}