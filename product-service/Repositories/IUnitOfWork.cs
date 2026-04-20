namespace product_service.Repositories;

public interface IUnitOfWork
{
    public IProductRepository Products { get; }
}