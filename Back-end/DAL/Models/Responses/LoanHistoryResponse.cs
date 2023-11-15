﻿namespace PL.Models.Responses
{
    public class LoanHistoryResponse
    {
        public string? Type { get; set; }
        public string? SerialNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int ProductId { get; set; }

    }
}
