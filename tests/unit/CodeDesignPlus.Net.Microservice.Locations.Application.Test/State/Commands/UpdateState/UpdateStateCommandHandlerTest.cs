using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Commands.UpdateState;

public class UpdateStateCommandHandlerTest
{
    private readonly Mock<IStateRepository> _repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IPubSub> _pubSubMock;
    private readonly UpdateStateCommandHandler _handler;
    private readonly FakeData fakeData = new();

    public UpdateStateCommandHandlerTest()
    {
        _repositoryMock = new Mock<IStateRepository>();
        _userContextMock = new Mock<IUserContext>();
        _pubSubMock = new Mock<IPubSub>();
        _handler = new UpdateStateCommandHandler(_repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateStateCommand request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);

    }

    [Fact]
    public async Task Handle_StateNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateStateCommand;
        _repositoryMock.Setup(r => r.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((StateAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.StateNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.StateNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CountryNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateStateCommand;
        var stateAggregate = fakeData.StateAggregate;
        _repositoryMock.Setup(r => r.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(stateAggregate);
        _repositoryMock.Setup(r => r.ExistsAsync<CountryAggregate>(request.IdCountry, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.CountryNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CountryNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesStateAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.UpdateStateCommand;
        var stateAggregate = fakeData.StateAggregate;
        _repositoryMock.Setup(r => r.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(stateAggregate);
        _repositoryMock.Setup(r => r.ExistsAsync<CountryAggregate>(request.IdCountry, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userContextMock.SetupGet(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.UpdateAsync(stateAggregate, It.IsAny<CancellationToken>()), Times.Once);
        _pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<StateUpdatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
