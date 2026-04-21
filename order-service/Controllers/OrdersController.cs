using Microsoft.AspNetCore.Mvc;
using order_service.Models;
using order_service.Services;

namespace order_service.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orders) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(orders.GetAllOrders());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var order = orders.GetById(id);
        return order != null ? Ok(order) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        var newOrder = await orders.PlaceOrder(order);
        if (newOrder != null)
        {
            return CreatedAtAction(nameof(Get), new { id = newOrder.Id }, newOrder);
        }

        return BadRequest("Order contains invalid product IDs");
    }
}