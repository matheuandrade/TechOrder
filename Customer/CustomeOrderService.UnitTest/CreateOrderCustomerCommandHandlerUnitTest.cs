using CustomerOrderService.Application.Order.Create;
using CustomerOrderService.Core.Entities.OrderErrors;
using CustomerOrderService.Core.External.Suppliers.Interfaces;
using CustomerOrderService.Core.External.Suppliers.Models;
using CustomerOrderService.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomeOrderService.UnitTest;

public class CreateOrderCustomerCommandHandlerUnitTest
{
    private readonly Mock<IRepository<CustomerOrderService.Core.Entities.Order>> _mockRepository;
    private readonly Mock<ISupplierApiService> _mockSupplierApiService;
    private readonly Mock<ILogger<CreateOrderCustomerCommand>> _mockLogger;
    private readonly CreateOrderCustomerCommandHandler _handler;

    public CreateOrderCustomerCommandHandlerUnitTest()
    {
        _mockRepository = new();
        _mockSupplierApiService = new();
        _mockLogger = new();

        _handler = new CreateOrderCustomerCommandHandler(
           _mockRepository.Object,
           _mockSupplierApiService.Object,
           _mockLogger.Object
       );
    }

    [Fact]
    public async Task Handle_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var cnpj = "12345678000195";

        var request = new CreateOrderCustomerCommand(cnpj, new CustomerOrderService.Application.Dto.OrderDto
        {
            CPF = "12345678901",
            Products =
                [
                    new() { ProductReference = "P1", Quantity = 10 },
                    new() { ProductReference = "P2", Quantity = 5 }
                ]
        });
        var supplierOrderId = Guid.NewGuid();

        _mockSupplierApiService
            .Setup(s => s.CreateSupplyOrder(It.IsAny<CreateOrderSupplierDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplierOrderId);

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<CustomerOrderService.Core.Entities.Order>(), It.IsAny<CancellationToken>()));

        _mockRepository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result);
        Assert.IsType<Guid>(result.Value);

        _mockLogger.Verify(l =>
            l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Starting to create order for customer")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockLogger.Verify(l =>
            l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Sending order to supplier with CNPJ")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockLogger.Verify(l =>
            l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Company created on supplier")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _mockLogger.Verify(l =>
            l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Order successfully created")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSupplierResponseIsNull()
    {
        // Arrange
        var cnpj = "12345678000195";

        var request = new CreateOrderCustomerCommand(cnpj, new CustomerOrderService.Application.Dto.OrderDto
        {
            CPF = "12345678901",
            Products =
                [
                    new() { ProductReference = "P1", Quantity = 10 },
                    new() { ProductReference = "P2", Quantity = 5 }
                ]
        });

        _mockSupplierApiService
            .Setup(s => s.CreateSupplyOrder(It.IsAny<CreateOrderSupplierDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(OrderErrors.CreateOrderOnSupplier, result.Error);
        _mockLogger.Verify(l =>
            l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Failed to create order on supplier for CNPJ")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionOccurs()
    {
        // Arrange
        var cnpj = "12345678000195";

        var request = new CreateOrderCustomerCommand(cnpj, new CustomerOrderService.Application.Dto.OrderDto
        {
            CPF = "12345678901",
            Products =
                [
                    new() { ProductReference = "P1", Quantity = 10 },
                    new() { ProductReference = "P2", Quantity = 5 }
                ]
        });

        _mockSupplierApiService
            .Setup(s => s.CreateSupplyOrder(It.IsAny<CreateOrderSupplierDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(OrderErrors.InternalError, result.Error);
        _mockLogger.Verify(l =>
        l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("An error occurred while processing the order for CNPJ")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }
}
