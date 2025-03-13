using Microsoft.Extensions.Logging;
using Moq;
using SupplierOrderService.Application.Reseller.Register;
using SupplierOrderService.Core.Interfaces;

namespace SupplierOrderService.UnitTests;

public class RegisterResellerCommandHandlerUnitTest
{
    private readonly Mock<IRepository<Core.Entities.Reseller>> _mockRepository;
    private readonly Mock<ILogger<RegisterResellerCommandHandler>> _mockLogger;
    private readonly RegisterResellerCommandHandler _handler;

    public RegisterResellerCommandHandlerUnitTest()
    {
        _mockRepository = new();
        _mockLogger = new();

        _handler = new RegisterResellerCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ShouldReturnErrorMessageForACompanyWhoAlreadyExists()
    {
        var cnpj = "cpnj";

        var command = new RegisterResellerCommand(new Application.Dtos.ResellerDto(cnpj));

        _mockRepository
        .Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Core.Entities.Reseller, bool>>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

        var response = await _handler.Handle(command, new CancellationToken());

        Assert.False(response.IsSuccess);
        Assert.NotNull(response.Error);
        Assert.Equal("Empresa já cadastrada", response.Error.Description);
    }
}
