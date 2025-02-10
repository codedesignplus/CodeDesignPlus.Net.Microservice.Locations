using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Commands.DeleteLocality;

public class DeleteLocalityCommandHandlerTest
{
    private readonly Mock<ILocalityRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteLocalityCommandHandler handler;
    private readonly FakeData fakeData = new();

    public DeleteLocalityCommandHandlerTest()
    {
        repositoryMock = new Mock<ILocalityRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteLocalityCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteLocalityCommand request = null!;

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
        var request = new DeleteLocalityCommand(fakeData.Locality.Id);
        repositoryMock
            .Setup(r => r.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LocalityAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.LocalityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.LocalityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesLocalityAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteLocalityCommand(fakeData.Locality.Id);
        var localityAggregate = fakeData.LocalityAggregate;
        repositoryMock
            .Setup(r => r.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(localityAggregate);
        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync<LocalityAggregate>(localityAggregate.Id, It.IsAny<CancellationToken>()), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<LocalityDeletedDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
    }
}
