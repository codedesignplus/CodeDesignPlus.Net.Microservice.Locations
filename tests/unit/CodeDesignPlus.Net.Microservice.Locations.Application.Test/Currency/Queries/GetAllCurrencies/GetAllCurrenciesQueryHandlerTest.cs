using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.GetAllCurrencies;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Queries.GetAllCurrencies;

public class GetAllCurrenciesQueryHandlerTest
{
    private readonly Mock<ICurrencyRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ICacheManager> cacheManagerMock;
    private readonly GetAllCurrenciesQueryHandler handler;
    private readonly FakeData fakeData = new();

    public GetAllCurrenciesQueryHandlerTest()
    {
        repositoryMock = new Mock<ICurrencyRepository>();
        mapperMock = new Mock<IMapper>();
        cacheManagerMock = new Mock<ICacheManager>();
        handler = new GetAllCurrenciesQueryHandler(repositoryMock.Object, mapperMock.Object, cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        GetAllCurrenciesQuery request = null!;

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
        var request = new GetAllCurrenciesQuery(null!);
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
