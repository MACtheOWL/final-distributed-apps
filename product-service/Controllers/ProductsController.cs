using Microsoft.AspNetCore.Mvc;
using product_service.Models;
using product_service.Repositories;

namespace product_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(IUnitOfWork unit) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(unit.Products.GetProducts());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var product = unit.Products.GetProduct(id);
        return product != null ? Ok(product) : NotFound();
    }

    [HttpPost]
    public IActionResult Post([FromBody] Product product)
    {
        var created = unit.Products.AddProduct(product);
        unit.Complete();
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Product product)
    {
        var updated = unit.Products.UpdateProduct(id, product);
        if (updated == null) return NotFound();
        unit.Complete();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!unit.Products.DeleteProduct(id)) return NotFound();
        unit.Complete();
        return NoContent();
    }
}
