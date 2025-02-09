using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Queries.FindCurrencyById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Queries.FindCurrencyById;

public class FindCurrencyByIdQueryHandlerTest
{
    private readonly Mock<ICurrencyRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheManager> _cacheManagerMock;
    private readonly FindCurrencyByIdQueryHandler _handler;
    private readonly FakeData fakeData = new();

    public FindCurrencyByIdQueryHandlerTest()
    {
        _repositoryMock = new Mock<ICurrencyRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheManagerMock = new Mock<ICacheManager>();
        _handler = new FindCurrencyByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        FindCurrencyByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));
        
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CurrencyExistsInCache_ReturnsCurrencyFromCache()
    {
        // Arrange
        var request = new FindCurrencyByIdQuery(fakeData.Currency.Id);
        var currencyDto = fakeData.Currency;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        _cacheManagerMock.Setup(x => x.GetAsync<CurrencyDto>(request.Id.ToString())).ReturnsAsync(currencyDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(currencyDto, result);
        _repositoryMock.Verify(x => x.FindAsync<CurrencyAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CurrencyNotInCache_FindsCurrencyInRepositoryAndCachesIt()
    {
        // Arrange
        var request = new FindCurrencyByIdQuery(fakeData.Currency.Id);
        var currency = fakeData.CurrencyAggregate;
        var currencyDto = fakeData.Currency;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        _repositoryMock.Setup(x => x.FindAsync<CurrencyAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(currency);
        _mapperMock.Setup(x => x.Map<CurrencyDto>(currency)).Returns(currencyDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(currencyDto, result);
        _cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), currencyDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CurrencyNotFoundInRepository_ThrowsCurrencyNotFoundException()
    {
        // Arrange
        var request = new FindCurrencyByIdQuery(fakeData.Currency.Id);
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        _repositoryMock.Setup(x => x.FindAsync<CurrencyAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((CurrencyAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.CurrencyNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CurrencyNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
