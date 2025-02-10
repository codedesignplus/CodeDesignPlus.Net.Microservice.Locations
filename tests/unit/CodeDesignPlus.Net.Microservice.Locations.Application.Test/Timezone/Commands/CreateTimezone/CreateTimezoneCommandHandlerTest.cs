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
    private readonly CreateTimezoneCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateTimezoneCommandHandlerTest()
    {
        repositoryMock = new Mock<ITimezoneRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateTimezoneCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
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
    public async Task Handle_TimezoneAlreadyExists_ThrowsCodeDesignPlusException()
    {
        var command = fakeData.CreateTimezoneCommand;
        repositoryMock.Setup(r => r.ExistsAsync<TimezoneAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        Assert.Equal(Errors.TimezoneAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.TimezoneAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesTimezoneAndPublishesEvents()
    {
        var command = fakeData.CreateTimezoneCommand;
        repositoryMock.Setup(r => r.ExistsAsync<TimezoneAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(r => r.CreateAsync(It.IsAny<TimezoneAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<TimezoneCreatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
