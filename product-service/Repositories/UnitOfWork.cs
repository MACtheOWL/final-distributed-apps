using product_service.Data;

namespace product_service.Repositories;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public IProductRepository Products { get; } = new ProductRepository(db);
}