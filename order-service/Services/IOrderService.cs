using order_service.Models;

namespace order_service.Services;

public interface IOrderService
{
    public List<Order> GetAllOrders();

    public Order? GetById(int id);

    public Task<Order?> PlaceOrder(Order order);
}
