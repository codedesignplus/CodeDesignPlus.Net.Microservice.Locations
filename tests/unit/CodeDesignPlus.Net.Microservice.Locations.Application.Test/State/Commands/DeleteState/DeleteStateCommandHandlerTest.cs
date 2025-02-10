using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Commands.DeleteState;

public class DeleteStateCommandHandlerTest
{
    private readonly Mock<IStateRepository> repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IPubSub> _pubSubMock;
    private readonly DeleteStateCommandHandler handler;
    private readonly FakeData fakeData = new();

    public DeleteStateCommandHandlerTest()
    {
        repositoryMock = new Mock<IStateRepository>();
        _userContextMock = new Mock<IUserContext>();
        _pubSubMock = new Mock<IPubSub>();
        handler = new DeleteStateCommandHandler(repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteStateCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_StateNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new DeleteStateCommand(fakeData.State.Id);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<StateAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((StateAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.StateNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.StateNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesStateAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteStateCommand(fakeData.State.Id);
        var cancellationToken = CancellationToken.None;
        var stateAggregate = fakeData.StateAggregate;

        repositoryMock.Setup(r => r.FindAsync<StateAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(stateAggregate);
        _userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync<StateAggregate>(stateAggregate.Id, cancellationToken), Times.Once);
        _pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<StateDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
