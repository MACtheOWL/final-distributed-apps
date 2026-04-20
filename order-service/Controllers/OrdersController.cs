using Microsoft.AspNetCore.Mvc;
using order_service.Models;
using order_service.Repositories;

namespace order_service.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IUnitOfWork unit) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(unit.Orders.GetOrders());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var order = unit.Orders.GetOrder(id);
        return order != null ? Ok(order) : NotFound();
    }

    [HttpPost]
    public IActionResult Post([FromBody] Order order)
    {
        return Ok();
    }
}