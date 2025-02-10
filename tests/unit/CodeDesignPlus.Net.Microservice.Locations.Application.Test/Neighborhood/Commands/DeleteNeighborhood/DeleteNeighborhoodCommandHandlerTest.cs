using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.DeleteNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Commands.DeleteNeighborhood;

public class DeleteNeighborhoodCommandHandlerTest
{
    private readonly Mock<INeighborhoodRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteNeighborhoodCommandHandler handler;
    private readonly FakeData fakeData = new();

    public DeleteNeighborhoodCommandHandlerTest()
    {
        repositoryMock = new Mock<INeighborhoodRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteNeighborhoodCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteNeighborhoodCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_NeighborhoodNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new DeleteNeighborhoodCommand(fakeData.Neighborhood.Id);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(repo => repo.FindAsync<NeighborhoodAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((NeighborhoodAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.NeighborhoodNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.NeighborhoodNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesNeighborhoodAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteNeighborhoodCommand(fakeData.Neighborhood.Id);
        var cancellationToken = CancellationToken.None;
        var neighborhoodAggregate = fakeData.NeighborhoodAggregate;

        repositoryMock
            .Setup(repo => repo.FindAsync<NeighborhoodAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(neighborhoodAggregate);

        userContextMock.Setup(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.DeleteAsync<NeighborhoodAggregate>(neighborhoodAggregate.Id, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<NeighborhoodDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
