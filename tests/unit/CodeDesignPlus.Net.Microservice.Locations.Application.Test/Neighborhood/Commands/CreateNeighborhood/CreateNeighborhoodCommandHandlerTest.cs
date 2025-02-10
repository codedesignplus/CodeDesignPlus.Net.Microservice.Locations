using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Commands.CreateNeighborhood;

public class CreateNeighborhoodCommandHandlerTest
{
    private readonly Mock<INeighborhoodRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly CreateNeighborhoodCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateNeighborhoodCommandHandlerTest()
    {
        repositoryMock = new Mock<INeighborhoodRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateNeighborhoodCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
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
    public async Task Handle_NeighborhoodAlreadyExists_ThrowsCodeDesignPlusException()
    {
        var command = fakeData.CreateNeighborhoodCommand;
        repositoryMock.Setup(r => r.ExistsAsync<NeighborhoodAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        Assert.Equal(Errors.NeighborhoodAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.NeighborhoodAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_LocalityNotFound_ThrowsCodeDesignPlusException()
    {
        var command = fakeData.CreateNeighborhoodCommand;
        repositoryMock.Setup(r => r.ExistsAsync<NeighborhoodAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<LocalityAggregate>(command.IdLocality, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        Assert.Equal(Errors.LocalityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.LocalityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesNeighborhoodAndPublishesEvents()
    {
        var command =  fakeData.CreateNeighborhoodCommand;
        repositoryMock.Setup(r => r.ExistsAsync<NeighborhoodAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<LocalityAggregate>(command.IdLocality, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        await handler.Handle(command, CancellationToken.None);

        repositoryMock.Verify(r => r.CreateAsync(It.IsAny<NeighborhoodAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<NeighborhoodCreatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
