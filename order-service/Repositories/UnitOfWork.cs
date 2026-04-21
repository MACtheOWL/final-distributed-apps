using order_service.Data;

namespace order_service.Repositories;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public IOrderRepository Orders { get; } = new OrderRepository(db);

    public void Complete()
    {
        db.SaveChanges();
    }
}