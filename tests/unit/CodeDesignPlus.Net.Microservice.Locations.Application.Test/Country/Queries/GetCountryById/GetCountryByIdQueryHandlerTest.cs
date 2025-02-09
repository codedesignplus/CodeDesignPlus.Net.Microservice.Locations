using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Queries.GetCountryById;

public class GetCountryByIdQueryHandlerTest
{
    private readonly Mock<ICountryRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheManager> _cacheManagerMock;
    private readonly GetCountryByIdQueryHandler _handler;
    private readonly FakeData fakeData = new();

    public GetCountryByIdQueryHandlerTest()
    {
        _repositoryMock = new Mock<ICountryRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheManagerMock = new Mock<ICacheManager>();
        _handler = new GetCountryByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        GetCountryByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_CountryExistsInCache_ReturnsCountryFromCache()
    {
        // Arrange
        var request = new GetCountryByIdQuery(fakeData.Country.Id);
        var cachedCountry = new CountryDto();
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        _cacheManagerMock.Setup(x => x.GetAsync<CountryDto>(request.Id.ToString())).ReturnsAsync(cachedCountry);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(cachedCountry, result);
        _repositoryMock.Verify(x => x.FindAsync<CountryAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CountryDoesNotExistInCache_ReturnsCountryFromRepository()
    {
        // Arrange
        var request = new GetCountryByIdQuery(fakeData.Country.Id);
        var countryAggregate = fakeData.CountryAggregate;
        var countryDto = fakeData.Country;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        _repositoryMock.Setup(x => x.FindAsync<CountryAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(countryAggregate);
        _mapperMock.Setup(x => x.Map<CountryDto>(countryAggregate)).Returns(countryDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(countryDto, result);
        _cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), countryDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CountryNotFoundInRepository_ThrowsCountryNotFoundException()
    {
        // Arrange
        var request = new GetCountryByIdQuery(fakeData.Country.Id);
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        _repositoryMock.Setup(x => x.FindAsync<CountryAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((CountryAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.CountryNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CountryNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
