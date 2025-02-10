using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Commands.CreateState;

public class CreateStateCommandHandlerTest
{
    private readonly Mock<IStateRepository> repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IPubSub> _pubSubMock;
    private readonly CreateStateCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateStateCommandHandlerTest()
    {
        repositoryMock = new Mock<IStateRepository>();
        _userContextMock = new Mock<IUserContext>();
        _pubSubMock = new Mock<IPubSub>();
        handler = new CreateStateCommandHandler(repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        CreateStateCommand request = null;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_StateAlreadyExists_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.CreateStateCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(request.Id, cancellationToken)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.StateAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.StateAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CountryNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.CreateStateCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<CountryAggregate>(request.IdCountry, cancellationToken)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.CountryNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CountryNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesStateAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.CreateStateCommand;
        var cancellationToken = CancellationToken.None;
        var userId = Guid.NewGuid();

        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<CountryAggregate>(request.IdCountry, cancellationToken)).ReturnsAsync(true);
        _userContextMock.Setup(u => u.IdUser).Returns(userId);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.CreateAsync(It.IsAny<StateAggregate>(), cancellationToken), Times.Once);
        _pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<StateCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
