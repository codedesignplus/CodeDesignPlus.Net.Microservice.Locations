using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.DeleteCountry;

public class DeleteCountryCommandHandlerTest
{
    private readonly Mock<ICountryRepository> repositoryMock;
    private readonly Mock<IUserContext> userContextMock;
    private readonly Mock<IPubSub> pubSubMock;
    private readonly DeleteCountryCommandHandler handler;
    private readonly FakeData fakeData = new();

    public DeleteCountryCommandHandlerTest()
    {
        repositoryMock = new Mock<ICountryRepository>();
        userContextMock = new Mock<IUserContext>();
        pubSubMock = new Mock<IPubSub>();
        handler = new DeleteCountryCommandHandler(repositoryMock.Object, userContextMock.Object, pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteCountryCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CountryNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.DeleteCountryCommand;
        var cancellationToken = CancellationToken.None;

        repositoryMock
            .Setup(r => r.FindAsync<CountryAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((CountryAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.CountryNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CountryNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesCountryAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.DeleteCountryCommand;
        var cancellationToken = CancellationToken.None;
        var countryAggregate = fakeData.CountryAggregate;

        repositoryMock
            .Setup(r => r.FindAsync<CountryAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(countryAggregate);

        userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync<CountryAggregate>(countryAggregate.Id, cancellationToken), Times.Once);
        pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<CountryDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
