using Xunit;
using Moq;
using order_service.Services;
using order_service.Models;
using order_service.Repositories;

public class OrderServiceTests
{
    [Fact]
    public async Task PlaceOrder_ReturnsNull_WhenProductDoesNotExist()
    {
        var mockProducts = new Mock<ProductServiceClient>(null!);
        var mockUnit = new Mock<IUnitOfWork>();

        mockProducts.Setup(p => p.GetById(It.IsAny<int>()))
                    .ReturnsAsync((Product?)null);

        var service = new OrderService(mockProducts.Object, mockUnit.Object);

        var order = new Order
        {
            ProductIds = new int[] { 1 }
        };

        var result = await service.PlaceOrder(order);

        Assert.Null(result);
    }

    [Fact]
    public async Task PlaceOrder_CalculatesTotalPrice()
    {
        var mockProducts = new Mock<ProductServiceClient>(null!);

        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
                .Returns((Order o) => o);

        var mockUnit = new Mock<IUnitOfWork>();
        mockUnit.Setup(u => u.Orders).Returns(mockRepo.Object);
        mockUnit.Setup(u => u.Complete());

        mockProducts.Setup(p => p.GetById(It.IsAny<int>()))
            .ReturnsAsync(new Product { Id = 1, Price = 50 });

        var service = new OrderService(mockProducts.Object, mockUnit.Object);

        var order = new Order
        {
            ProductIds = new int[] { 1, 1 }
        };

        var result = await service.PlaceOrder(order);

        Assert.NotNull(result);
        Assert.Equal(100, result.TotalPrice);
    }

    [Fact]
    public async Task PlaceOrder_MultipleProducts_AddsCorrectly()
    {
        var mockProducts = new Mock<ProductServiceClient>(null!);

        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
                .Returns((Order o) => o);

        var mockUnit = new Mock<IUnitOfWork>();
        mockUnit.Setup(u => u.Orders).Returns(mockRepo.Object);
        mockUnit.Setup(u => u.Complete());

        mockProducts.Setup(p => p.GetById(It.IsAny<int>()))
            .ReturnsAsync(new Product { Price = 25 });

        var service = new OrderService(mockProducts.Object, mockUnit.Object);

        var order = new Order
        {
            ProductIds = new int[] { 1, 2, 3 }
        };

        var result = await service.PlaceOrder(order);

        Assert.Equal(75, result.TotalPrice);
    }

    [Fact]
    public async Task PlaceOrder_EmptyProductList_ReturnsZeroTotal()
    {
        var mockProducts = new Mock<ProductServiceClient>(null!);

        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.AddOrder(It.IsAny<Order>()))
                .Returns((Order o) => o);

        var mockUnit = new Mock<IUnitOfWork>();
        mockUnit.Setup(u => u.Orders).Returns(mockRepo.Object);
        mockUnit.Setup(u => u.Complete());

        var service = new OrderService(mockProducts.Object, mockUnit.Object);

        var order = new Order
        {
            ProductIds = new int[] { }
        };

        var result = await service.PlaceOrder(order);

        Assert.Equal(0, result.TotalPrice);
    }
}