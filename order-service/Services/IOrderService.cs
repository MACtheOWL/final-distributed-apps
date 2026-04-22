using order_service.Models;

namespace order_service.Services;

public interface IOrderService
{
    public List<Order> GetAllOrders();

    public Order? GetById(int id);

    public Task<Order?> PlaceOrder(Order order);

    public Task<Order?> UpdateOrder(int id, Order order);

    public bool DeleteOrder(int id);
}
