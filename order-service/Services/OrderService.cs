using order_service.Models;
using order_service.Repositories;

namespace order_service.Services;

public class OrderService(ProductServiceClient products, IUnitOfWork unit) : IOrderService
{
    public List<Order> GetAllOrders()
    {
        return unit.Orders.GetOrders();
    }

    public Order? GetById(int id)
    {
        return unit.Orders.GetOrder(id);
    }

    public async Task<Order?> PlaceOrder(Order order)
    {
        var total = await CalculateTotal(order.ProductIds);
        if (total == null) return null;

        order.TotalPrice = total.Value;
        order = unit.Orders.AddOrder(order);
        unit.Complete();

        return order;
    }

    public async Task<Order?> UpdateOrder(int id, Order order)
    {
        var total = await CalculateTotal(order.ProductIds);
        if (total == null) return null;

        order.TotalPrice = total.Value;

        var updated = unit.Orders.UpdateOrder(id, order);
        if (updated == null) return null;

        unit.Complete();

        return updated;
    }

    public bool DeleteOrder(int id)
    {
        if (!unit.Orders.DeleteOrder(id)) return false;
        unit.Complete();
        return true;
    }

    private async Task<decimal?> CalculateTotal(int[] productIds)
    {
        decimal total = 0;

        foreach (var id in productIds)
        {
            var product = await products.GetById(id);
            if (product == null) return null;
            total += product.Price;
        }

        return total;
    }
}
