using product_service.Data;
using product_service.Models;

namespace product_service.Repositories;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public List<Product> GetProducts()
    {
        return db.Products.ToList();
    }

    public Product? GetProduct(int id)
    {
        return db.Products.Find(id);
    }
}