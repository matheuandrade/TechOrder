using Microsoft.Extensions.Logging;
using Moq;
using ResellerService.Aplication.Reseller.Register;
using ResellerService.Core.External.Supplier;
using ResellerService.Core.Interfaces;
using System.Linq.Expressions;

namespace Reseller.UnitTest;

public class RegisterResellerCommandHandlerUnitTest
{
    private readonly Mock<IRepository<ResellerService.Core.Entities.ResellerAggregate.Reseller>> _resellerRepositoryMock;
    private readonly Mock<ISupplierApiService> _suplierApiServiceMock;
    private readonly Mock<ILogger<RegisterResellerCommandHandler>> _loggerMock;
    private readonly RegisterResellerCommandHandler _handler;

    public RegisterResellerCommandHandlerUnitTest()
    {
        // Set up the mock repository
        _resellerRepositoryMock = new();
        _suplierApiServiceMock = new();
        _loggerMock = new();

        // Instantiate the handler with the mocked repository
        _handler = new RegisterResellerCommandHandler(_resellerRepositoryMock.Object, _suplierApiServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ShouldReturnErrorMessageForACompanyWhoAlreadyExists()
    {
        var cnpj = "cpnj";

        var command = new RegisterResellerCommand(new ResellerService.Aplication.Dtos.ResellerDto
        {
            CNPJ = cnpj,
        });

        _resellerRepositoryMock
        .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ResellerService.Core.Entities.ResellerAggregate.Reseller, bool>>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

        var response = await _handler.Handle(command, new CancellationToken());

        Assert.False(response.IsSuccess);
        Assert.NotNull(response.Error);
        Assert.Equal("Empresa já cadastrada", response.Error.Description);
    }

    [Fact]
    public async Task ShouldRegisterCompanySuccessfully()
    {
        var cnpj = "cpnj";

        var command = new RegisterResellerCommand(new ResellerService.Aplication.Dtos.ResellerDto
        {
            CNPJ = cnpj,
        });

        _resellerRepositoryMock
        .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ResellerService.Core.Entities.ResellerAggregate.Reseller, bool>>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);

        var response = await _handler.Handle(command, new CancellationToken());

        Assert.True(response.IsSuccess);
        Assert.IsType<SharedKernel.Result<Guid>>(response);
    }
}
