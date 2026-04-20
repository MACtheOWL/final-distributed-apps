using System.ComponentModel.DataAnnotations;

namespace product_service.Models;

public class Product
{
    [Key] public int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }
}