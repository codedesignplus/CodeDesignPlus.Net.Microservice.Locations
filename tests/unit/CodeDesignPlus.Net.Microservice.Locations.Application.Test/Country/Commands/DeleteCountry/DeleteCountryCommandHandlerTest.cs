using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.DeleteCountry;

public class DeleteCountryCommandHandlerTest
{
    private readonly Mock<ICountryRepository> _repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IPubSub> _pubSubMock;
    private readonly DeleteCountryCommandHandler _handler;
    private readonly FakeData fakeData = new();

    public DeleteCountryCommandHandlerTest()
    {
        _repositoryMock = new Mock<ICountryRepository>();
        _userContextMock = new Mock<IUserContext>();
        _pubSubMock = new Mock<IPubSub>();
        _handler = new DeleteCountryCommandHandler(_repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        DeleteCountryCommand request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));

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

        _repositoryMock
            .Setup(r => r.FindAsync<CountryAggregate>(request.Id, cancellationToken))
            .ReturnsAsync((CountryAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));

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

        _repositoryMock
            .Setup(r => r.FindAsync<CountryAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(countryAggregate);

        _userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

        // Act
        await _handler.Handle(request, cancellationToken);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync<CountryAggregate>(countryAggregate.Id, cancellationToken), Times.Once);
        _pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<CountryDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
