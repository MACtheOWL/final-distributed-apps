namespace order_service.Repositories;

public interface IUnitOfWork
{
    public IOrderRepository Orders { get; }
}