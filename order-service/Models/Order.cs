using System.ComponentModel.DataAnnotations;

namespace order_service.Models;

public class Order
{
    [Key] public int Id { get; set; }

    public int[] ProductIds { get; set; } = [];

    [Required]
    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }
}