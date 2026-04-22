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

    public Order? UpdateOrder(int id, Order order)
    {
        var existing = db.Orders.Find(id);
        if (existing == null) return null;

        existing.ProductIds = order.ProductIds;
        existing.OrderDate = order.OrderDate;
        existing.TotalPrice = order.TotalPrice;

        return existing;
    }

    public bool DeleteOrder(int id)
    {
        var existing = db.Orders.Find(id);
        if (existing == null) return false;

        db.Orders.Remove(existing);
        return true;
    }
}
