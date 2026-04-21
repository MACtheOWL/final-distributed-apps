using order_service.Models;

namespace order_service.Repositories;

public interface IOrderRepository
{
    public List<Order> GetOrders();

    public Order? GetOrder(int id);

    public Order AddOrder(Order order);
}