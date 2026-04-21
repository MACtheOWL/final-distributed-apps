using order_service.Data;
using order_service.Models;

namespace order_service.Repositories;

public class OrderRepository(AppDbContext db) : IOrderRepository
{
    public List<Order> GetOrders()
    {
        return db.Orders.ToList();
    }

    public Order? GetOrder(int id)
    {
        return db.Orders.Find(id);
    }

    public Order AddOrder(Order order)
    {
        db.Orders.Add(order);
        return order;
    }
}