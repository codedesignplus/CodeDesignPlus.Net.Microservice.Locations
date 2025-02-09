using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Queries.FindAllCurrencies;

public class FindAllCurrenciesQueryHandlerTest
{
    private readonly Mock<ICurrencyRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindAllCurrenciesQueryHandler _handler;
    private readonly FakeData fakeData = new();

    public FindAllCurrenciesQueryHandlerTest()
    {
        _repositoryMock = new Mock<ICurrencyRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new FindAllCurrenciesQueryHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        FindAllCurrenciesQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsMappedCurrencies()
    {
        // Arrange
        var request = new FindAllCurrenciesQuery(null!);
        var currencyAggregates = new List<CurrencyAggregate> { fakeData.CurrencyAggregate };
        var currencyDtos = new List<CurrencyDto> { fakeData.Currency };

        _repositoryMock
            .Setup(repo => repo.MatchingAsync<CurrencyAggregate>(request.Criteria, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currencyAggregates);

        _mapperMock
            .Setup(mapper => mapper.Map<List<CurrencyDto>>(currencyAggregates))
            .Returns(currencyDtos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(currencyDtos, result);
    }
}
