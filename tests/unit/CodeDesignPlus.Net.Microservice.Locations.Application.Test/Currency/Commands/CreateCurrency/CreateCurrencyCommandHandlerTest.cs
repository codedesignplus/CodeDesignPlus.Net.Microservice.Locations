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
    private readonly Mock<ICurrencyRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
        private readonly Mock<ICacheManager> cacheMock = new();
    private readonly CreateCurrencyCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateCurrencyCommandHandlerTest()
    {
        repositoryMock = new Mock<ICurrencyRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateCurrencyCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object, cacheMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(null!, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CurrencyAlreadyExists_IsTheRetryAndDoesNotCreateItAgain()
    {
        var command = fakeData.CreateCurrencyCommand;
        repositoryMock.Setup(repo => repo.ExistsAsync<CurrencyAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act: el mismo id es el reintento del mismo formulario (pendings/589, regla 66), no un error.
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryMock.Verify(x => x.CreateIfAbsentAsync(It.IsAny<CurrencyAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesCurrencyAndPublishesEvents()
    {
        var command = fakeData.CreateCurrencyCommand;
        repositoryMock.Setup(repo => repo.ExistsAsync<CurrencyAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        userContextMock.Setup(user => user.IdUser).Returns(Guid.NewGuid());

        await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(repo => repo.CreateIfAbsentAsync(It.IsAny<CurrencyAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<CurrencyCreatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
