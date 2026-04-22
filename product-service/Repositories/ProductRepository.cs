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

    public Product AddProduct(Product product)
    {
        db.Products.Add(product);
        return product;
    }

    public Product? UpdateProduct(int id, Product product)
    {
        var existing = db.Products.Find(id);
        if (existing == null) return null;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;

        return existing;
    }

    public bool DeleteProduct(int id)
    {
        var existing = db.Products.Find(id);
        if (existing == null) return false;

        db.Products.Remove(existing);
        return true;
    }
}
