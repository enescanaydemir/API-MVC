using System;

namespace example_api.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string Category { get; set; }
    public int Discount { get; set; }
    public bool? OnSale { get; set; }
    public bool? Popular { get; set; }
}
