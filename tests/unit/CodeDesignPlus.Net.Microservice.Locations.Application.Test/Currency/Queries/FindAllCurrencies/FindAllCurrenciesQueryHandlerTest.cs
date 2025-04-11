using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindAllCurrencies;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Queries.FindAllCurrencies;

public class FindAllCurrenciesQueryHandlerTest
{
    private readonly Mock<ICurrencyRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly FindAllCurrenciesQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindAllCurrenciesQueryHandlerTest()
    {
        repositoryMock = new Mock<ICurrencyRepository>();
        mapperMock = new Mock<IMapper>();
        handler = new FindAllCurrenciesQueryHandler(repositoryMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        FindAllCurrenciesQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

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

        var pagination = Pagination<CurrencyAggregate>.Create(currencyAggregates, currencyAggregates.Count, 10, 0);

        repositoryMock
            .Setup(repo => repo.MatchingAsync<CurrencyAggregate>(request.Criteria, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagination);

        mapperMock
            .Setup(mapper => mapper.Map<Pagination<CurrencyDto>>(pagination))
            .Returns(Pagination<CurrencyDto>.Create(currencyDtos, currencyDtos.Count, 10, 0));

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(currencyDtos, result.Data);
    }
}
