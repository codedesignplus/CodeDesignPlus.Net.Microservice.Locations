using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Commands.CreateCity;

public class CreateCityCommandHandlerTest
{
    private readonly Mock<ICityRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly CreateCityCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateCityCommandHandlerTest()
    {
        repositoryMock = new Mock<ICityRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateCityCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        CreateCityCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CityAlreadyExists_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.CreateCityCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.Id, cancellationToken)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.CityAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.CityAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_StateNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.CreateCityCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(request.IdState, cancellationToken)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.StateNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.StateNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesCityAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.CreateCityCommand;
        var cancellationToken = CancellationToken.None;
        var userId = Guid.NewGuid();

        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<StateAggregate>(request.IdState, cancellationToken)).ReturnsAsync(true);
        userContextMock.Setup(u => u.IdUser).Returns(userId);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.CreateAsync(It.IsAny<CityAggregate>(), cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<CityCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
