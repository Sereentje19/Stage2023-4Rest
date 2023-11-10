﻿namespace Stage4rest2023.Models.DTOs;

public class ProductResponse
{
    public int ProductId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Type { get; set; }
    public string? SerialNumber { get; set; }
}