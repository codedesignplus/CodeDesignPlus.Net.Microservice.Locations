using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Commands.UpdateLocality;

public class UpdateLocalityCommandHandlerTest
{
    private readonly Mock<ILocalityRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateLocalityCommandHandler handler;
    private readonly FakeData fakeData = new();

    public UpdateLocalityCommandHandlerTest()
    {
        repositoryMock = new Mock<ILocalityRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateLocalityCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateLocalityCommand request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_LocalityNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateLocalityCommand;
        repositoryMock.Setup(r => r.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((LocalityAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.LocalityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.LocalityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CityNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateLocalityCommand;
        var localityAggregate = fakeData.LocalityAggregate;
        repositoryMock.Setup(r => r.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(localityAggregate);
        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.IdCity, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.CityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesLocalityAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.UpdateLocalityCommand;
        var localityAggregate = fakeData.LocalityAggregate;
        repositoryMock.Setup(r => r.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(localityAggregate);
        repositoryMock.Setup(r => r.ExistsAsync<CityAggregate>(request.IdCity, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(localityAggregate, It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<LocalityUpdatedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
