using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var orderServiceUrl = Environment.GetEnvironmentVariable("ORDER_SERVICE_URL") ?? "http://localhost:3000";
var productServiceUrl = Environment.GetEnvironmentVariable("PRODUCT_SERVICE_URL") ?? "http://localhost:3001";

var http = new HttpClient();
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters = { new JsonStringEnumConverter() }
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Create order");
    Console.WriteLine("2. View order by ID");
    Console.WriteLine("0. Exit");

    var choice = PromptInput("Choice (number)");
    Console.WriteLine();

    switch (choice)
    {
        case "0": break;
        case "1": await CreateOrder(); break;
        case "2": await ViewOrder(); break;
        default: Console.WriteLine("Invalid choice."); break;
    }
}

async Task CreateOrder()
{
    List<Product> products;
    try
    {
        var res = await http.GetAsync($"{productServiceUrl}/products");
        res.EnsureSuccessStatusCode();
        products = JsonSerializer.Deserialize<List<Product>>(await res.Content.ReadAsStringAsync(), jsonOptions) ?? [];
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to fetch products: {ex.Message}");
        return;
    }

    if (products.Count == 0)
    {
        Console.WriteLine("No products available.");
        return;
    }

    Console.WriteLine("Available products:");
    for (int i = 0; i < products.Count; i++)
        Console.WriteLine($"  {i + 1}. {products[i]}");

    Console.WriteLine();
    var input = PromptInput("Enter product numbers (comma-separated)");
    var selectedIds = new List<int>();

    foreach (var part in input.Split(','))
    {
        if (int.TryParse(part.Trim(), out int idx) && idx >= 1 && idx <= products.Count)
            selectedIds.Add(products[idx - 1].Id);
        else
            Console.WriteLine($"  Skipping invalid entry: '{part.Trim()}'");
    }

    if (selectedIds.Count == 0)
    {
        Console.WriteLine("No valid products selected.");
        return;
    }

    var order = new { ProductIds = selectedIds, OrderDate = DateTime.UtcNow };
    var body = new StringContent(JsonSerializer.Serialize(order), Encoding.UTF8, "application/json");

    try
    {
        var res = await http.PostAsync($"{orderServiceUrl}/orders", body);
        res.EnsureSuccessStatusCode();
        var created = JsonSerializer.Deserialize<Order>(await res.Content.ReadAsStringAsync(), jsonOptions);
        Console.WriteLine($"Order created: {created!}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to create order: {ex.Message}");
    }
}

async Task ViewOrder()
{
    var input = PromptInput("Order ID");
    if (!int.TryParse(input, out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    try
    {
        var res = await http.GetAsync($"{orderServiceUrl}/orders/{id}");
        if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"Order {id} not found.");
            return;
        }
        res.EnsureSuccessStatusCode();
        var order = JsonSerializer.Deserialize<Order>(await res.Content.ReadAsStringAsync(), jsonOptions);
        Console.WriteLine(order!.ToString());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to fetch order: {ex.Message}");
    }
}

string PromptInput(string label)
{
    Console.Write($"{label}: ");
    return Console.ReadLine()?.Trim() ?? "";
}

record Product(int Id, string Name, string? Description, decimal Price)
{
    public override string ToString() => $"[{Id}] {Name} - ${Price:F2}{(Description != null ? $" ({Description})" : "")}";
}

record Order(int Id, int[] ProductIds, DateTime OrderDate, decimal TotalPrice)
{
    public override string ToString() => $"Order #{Id} | Date: {OrderDate:yyyy-MM-dd HH:mm} UTC | Products: [{string.Join(", ", ProductIds)}] | Total: ${TotalPrice:F2}";
}
