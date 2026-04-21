namespace order_service.Services;

public class ProductServiceClient(HttpClient client)
{
    public Task<List<Product>> GetAll()
    {
        return client.GetFromJsonAsync<List<Product>>("/products")!;
    }

    public virtual async Task<Product?> GetById(int id)
    {
        var response = await client.GetAsync($"/products/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        return null;
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}