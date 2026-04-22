using product_service.Models;

namespace product_service.Repositories;

public interface IProductRepository
{
    public List<Product> GetProducts();

    public Product? GetProduct(int id);

    public Product AddProduct(Product product);

    public Product? UpdateProduct(int id, Product product);

    public bool DeleteProduct(int id);
}
