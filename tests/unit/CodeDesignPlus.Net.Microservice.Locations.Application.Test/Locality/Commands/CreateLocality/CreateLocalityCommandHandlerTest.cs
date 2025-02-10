using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Commands.CreateLocality;

public class CreateLocalityCommandHandlerTest
{
    private readonly Mock<ILocalityRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly CreateLocalityCommandHandler handler;
    private readonly FakeData fakeData = new();

    public CreateLocalityCommandHandlerTest()
    {
        repositoryMock = new Mock<ILocalityRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new CreateLocalityCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        CreateLocalityCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_LocalityAlreadyExists_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.CreateLocalityCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<LocalityAggregate>(request.Id, cancellationToken)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.LocalityAlreadyExists.GetMessage(), exception.Message);
        Assert.Equal(Errors.LocalityAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CityNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.CreateLocalityCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<LocalityAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.IdCity, cancellationToken)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.CityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesLocalityAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.CreateLocalityCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock.Setup(r => r.ExistsAsync<LocalityAggregate>(request.Id, cancellationToken)).ReturnsAsync(false);
        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.IdCity, cancellationToken)).ReturnsAsync(true);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.CreateAsync(It.IsAny<LocalityAggregate>(), cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<LocalityCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
