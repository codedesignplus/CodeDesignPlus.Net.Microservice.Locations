using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Commands.UpdateTimezone;

public class UpdateTimezoneCommandHandlerTest
{
    private readonly Mock<ITimezoneRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly UpdateTimezoneCommandHandler handler;
    private readonly FakeData fakeData = new();

    public UpdateTimezoneCommandHandlerTest()
    {
        repositoryMock = new Mock<ITimezoneRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new UpdateTimezoneCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateTimezoneCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_TimezoneNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateTimezoneCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<TimezoneAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((TimezoneAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.TimezoneNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.TimezoneNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesTimezoneAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.UpdateTimezoneCommand;
        var cancellationToken = CancellationToken.None;
        var timezoneAggregate = fakeData.TimezoneAggregate;

        repositoryMock
            .Setup(r => r.FindAsync<TimezoneAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(timezoneAggregate);

        userContextMock.SetupGet(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(timezoneAggregate, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<TimezoneUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
