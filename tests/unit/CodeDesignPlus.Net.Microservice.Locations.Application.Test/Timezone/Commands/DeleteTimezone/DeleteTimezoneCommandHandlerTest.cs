using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.DeleteTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Commands.DeleteTimezone;

public class DeleteTimezoneCommandHandlerTest
{
    private readonly Mock<ITimezoneRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteTimezoneCommandHandler handler;
    private readonly FakeData fakeData = new();

    public DeleteTimezoneCommandHandlerTest()
    {
        repositoryMock = new Mock<ITimezoneRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteTimezoneCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteTimezoneCommand request = null!;
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
        var request = new DeleteTimezoneCommand(fakeData.Timezone.Id);
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(repo => repo.FindAsync<TimezoneAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((TimezoneAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.TimezoneNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.TimezoneNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesTimezoneAndPublishesEvents()
    {
        // Arrange
        var request = new DeleteTimezoneCommand(fakeData.Timezone.Id);
        var cancellationToken = CancellationToken.None;
        var aggregate = fakeData.TimezoneAggregate;

        repositoryMock.Setup(repo => repo.FindAsync<TimezoneAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(aggregate);

        userContextMock.Setup(user => user.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.DeleteAsync<TimezoneAggregate>(aggregate.Id, cancellationToken), Times.Once);
        pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<TimezoneCreatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
