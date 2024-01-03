﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public byte[] File { get; set; }
        public string FileType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        [Column(TypeName = "nvarchar(24)")]
        public ProductType Type { get; set; }
        public string SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeDeleted { get; set; }
        public ICollection<LoanHistory> LoanHistory { get; set; }
    }
}
