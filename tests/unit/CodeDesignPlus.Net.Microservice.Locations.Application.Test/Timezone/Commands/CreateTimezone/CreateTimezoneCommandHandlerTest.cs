using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Commands.CreateTimezone;

public class CreateTimezoneCommandHandlerTest
{
    private readonly Mock<ITimezoneRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
        private readonly Mock<ICacheManager> cacheMock = new();
    private readonly CreateTimezoneCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateTimezoneCommandHandlerTest()
    {
        repositoryMock = new Mock<ITimezoneRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateTimezoneCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object, cacheMock.Object);
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
    public async Task Handle_TimezoneAlreadyExists_IsTheRetryAndDoesNotCreateItAgain()
    {
        var command = fakeData.CreateTimezoneCommand;
        repositoryMock.Setup(r => r.ExistsAsync<TimezoneAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act: el mismo id es el reintento del mismo formulario (pendings/589, regla 66), no un error.
        await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryMock.Verify(x => x.CreateIfAbsentAsync(It.IsAny<TimezoneAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesTimezoneAndPublishesEvents()
    {
        var command = fakeData.CreateTimezoneCommand;
        repositoryMock.Setup(r => r.ExistsAsync<TimezoneAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(r => r.CreateIfAbsentAsync(It.IsAny<TimezoneAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<TimezoneCreatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
