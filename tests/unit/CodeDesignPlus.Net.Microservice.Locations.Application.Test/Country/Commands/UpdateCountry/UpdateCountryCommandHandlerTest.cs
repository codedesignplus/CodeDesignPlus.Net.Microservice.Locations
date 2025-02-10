using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.UpdateCountry;
public class UpdateCountryCommandHandlerTest
{
    private readonly Mock<ICountryRepository> repositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IPubSub> _pubSubMock;
    private readonly UpdateCountryCommandHandler handler;
    private readonly FakeData fakeData = new();

    public UpdateCountryCommandHandlerTest()
    {
        repositoryMock = new Mock<ICountryRepository>();
        _userContextMock = new Mock<IUserContext>();
        _pubSubMock = new Mock<IPubSub>();
        handler = new UpdateCountryCommandHandler(repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        UpdateCountryCommand request = null!;
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
        var request = fakeData.UpdateCountryCommand;
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
    public async Task Handle_CurrencyNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = fakeData.UpdateCountryCommand;
        var cancellationToken = CancellationToken.None;
        var countryAggregate = fakeData.CountryAggregate;

        repositoryMock
            .Setup(r => r.FindAsync<CountryAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(countryAggregate);
        repositoryMock
            .Setup(r => r.ExistsAsync<CurrencyAggregate>(request.IdCurrency, cancellationToken))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.CurrencyNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CurrencyNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesCountryAndPublishesEvents()
    {
        // Arrange
        var request = fakeData.UpdateCountryCommand;
        var cancellationToken = CancellationToken.None;
        var countryAggregate = fakeData.CountryAggregate;
        var userId = Guid.NewGuid();

        repositoryMock
            .Setup(r => r.FindAsync<CountryAggregate>(request.Id, cancellationToken))
            .ReturnsAsync(countryAggregate);
        repositoryMock
            .Setup(r => r.ExistsAsync<CurrencyAggregate>(request.IdCurrency, cancellationToken))
            .ReturnsAsync(true);
        _userContextMock
            .Setup(u => u.IdUser).Returns(userId);

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.UpdateAsync(countryAggregate, cancellationToken), Times.Once);
        _pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<CountryUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
    }
}
