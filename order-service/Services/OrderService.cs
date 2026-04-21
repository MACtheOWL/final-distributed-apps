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
        decimal total = 0;

        foreach (var id in order.ProductIds)
        {
            var product = await products.GetById(id);

            if (product == null)
            {
                return null;
            }

            total += product.Price;
        }

        order.TotalPrice = total;

        order = unit.Orders.AddOrder(order);
        unit.Complete();

        return order;
    }
}