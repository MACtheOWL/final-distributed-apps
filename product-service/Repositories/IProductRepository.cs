using product_service.Models;

namespace product_service.Repositories;

public interface IProductRepository
{
    public List<Product> GetProducts();

    public Product? GetProduct(int id);
}