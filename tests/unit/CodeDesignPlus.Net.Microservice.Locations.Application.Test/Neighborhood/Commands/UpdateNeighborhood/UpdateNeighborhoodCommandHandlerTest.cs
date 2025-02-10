using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Commands.UpdateNeighborhood;

public class UpdateNeighborhoodCommandHandlerTest
{
    private readonly Mock<INeighborhoodRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateNeighborhoodCommandHandler handler;
    private readonly FakeData fakeData = new();

    public UpdateNeighborhoodCommandHandlerTest()
    {
        repositoryMock = new Mock<INeighborhoodRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateNeighborhoodCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateNeighborhoodCommand request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_NeighborhoodNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateNeighborhoodCommand;
        repositoryMock
            .Setup(r => r.FindAsync<NeighborhoodAggregate>(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((NeighborhoodAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.NeighborhoodNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.NeighborhoodNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_LocalityNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateNeighborhoodCommand;
        var neighborhoodAggregate = fakeData.NeighborhoodAggregate;
        repositoryMock
            .Setup(r => r.FindAsync<NeighborhoodAggregate>(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(neighborhoodAggregate);
        repositoryMock
            .Setup(r => r.ExistsAsync<LocalityAggregate>(request.IdLocality, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.LocalityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.LocalityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesNeighborhoodAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.UpdateNeighborhoodCommand;
        var neighborhoodAggregate = fakeData.NeighborhoodAggregate;
        repositoryMock
            .Setup(r => r.FindAsync<NeighborhoodAggregate>(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(neighborhoodAggregate);
        repositoryMock
            .Setup(r => r.ExistsAsync<LocalityAggregate>(request.IdLocality, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(neighborhoodAggregate, It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<NeighborhoodUpdatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
