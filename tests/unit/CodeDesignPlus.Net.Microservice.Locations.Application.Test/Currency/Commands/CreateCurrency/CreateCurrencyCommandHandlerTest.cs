using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.CreateCurrency;

public class CreateCurrencyCommandHandlerTest
{
    private readonly Mock<ICurrencyRepository> _repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IPubSub> _pubSubMock;
    private readonly CreateCurrencyCommandHandler _handler;
    private readonly FakeData fakeData = new();

    public CreateCurrencyCommandHandlerTest()
    {
        _repositoryMock = new Mock<ICurrencyRepository>();
        _userContextMock = new Mock<IUserContext>();
        _pubSubMock = new Mock<IPubSub>();
        _handler = new CreateCurrencyCommandHandler(_repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(null!, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CurrencyAlreadyExists_ThrowsCodeDesignPlusException()
    {
        var command = fakeData.CreateCurrencyCommand;
        _repositoryMock.Setup(repo => repo.ExistsAsync<CurrencyAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(command, CancellationToken.None));

        Assert.Equal(Errors.CurrencyAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.CurrencyAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesCurrencyAndPublishesEvents()
    {
        var command = fakeData.CreateCurrencyCommand;
        _repositoryMock.Setup(repo => repo.ExistsAsync<CurrencyAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _userContextMock.Setup(user => user.IdUser).Returns(Guid.NewGuid());

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<CurrencyAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        _pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<CurrencyCreatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
