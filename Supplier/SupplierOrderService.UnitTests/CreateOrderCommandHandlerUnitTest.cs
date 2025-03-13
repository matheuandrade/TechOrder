using Microsoft.Extensions.Logging;
using Moq;
using SupplierOrderService.Application.Order.Create;
using SupplierOrderService.Core.Entities.Errors;
using SupplierOrderService.Core.Interfaces;

namespace SupplierOrderService.UnitTests;

public class CreateOrderCommandHandlerUnitTest
{
    private readonly Mock<IRepository<Core.Entities.Order>> _mockOrderRepository;
    private readonly Mock<IRepository<Core.Entities.Reseller>> _mockResellerRepository;
    private readonly Mock<ILogger<CreateOrderCommandHandler>> _mockLogger;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerUnitTest()
    {
        _mockOrderRepository = new();
        _mockResellerRepository = new();
        _mockLogger = new();

        _handler = new CreateOrderCommandHandler(_mockOrderRepository.Object, _mockResellerRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenResellerDoesNotExist()
    {
        // Arrange
        var request = new CreateOrderCommand(
        new Application.Dto.OrderDto
        {
            CNPJ = "12345678000199",
            Products =
                [
                    new Application.Dto.ProductDto { ProductReference = "PROD123", Quantity = 2 }
                ]
        });
        var cancellationToken = CancellationToken.None;

        _mockResellerRepository
            .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Core.Entities.Reseller, bool>>>(), cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(result.Error, ResellerErrors.CNPJNotExists);

        _mockLogger.Verify(l =>
            l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Order creation failed: Reseller with CNPJ")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrder_WhenResellerExists()
    {
        // Arrange
        var request = new CreateOrderCommand(
        new Application.Dto.OrderDto
        {
            CNPJ = "12345678000199",
            Products =
                [
                    new Application.Dto.ProductDto { ProductReference = "PROD123", Quantity = 2 }
                ]
        });
        var cancellationToken = CancellationToken.None;

        _mockResellerRepository
            .Setup(r => r.AnyAsync(It.IsAny< System.Linq.Expressions.Expression<Func<Core.Entities.Reseller, bool>>>(), cancellationToken))
            .ReturnsAsync(true);

        _mockOrderRepository
            .Setup(r => r.AddAsync(It.IsAny<Core.Entities.Order>(), cancellationToken));

        _mockOrderRepository
            .Setup(r => r.SaveChangesAsync(cancellationToken));

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<Guid>(result.Value);

        _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Core.Entities.Order>(), cancellationToken), Times.Once);
        _mockOrderRepository.Verify(r => r.SaveChangesAsync(cancellationToken), Times.Once);
    }
}
