using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Queries.GetCountryById;

public class GetCountryByIdQueryHandlerTest
{
    private readonly Mock<ICountryRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ICacheManager> cacheManagerMock;
    private readonly GetCountryByIdQueryHandler handler;
    private readonly FakeData fakeData = new();

    public GetCountryByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<ICountryRepository>();
        mapperMock = new Mock<IMapper>();
        cacheManagerMock = new Mock<ICacheManager>();
        handler = new GetCountryByIdQueryHandler(repositoryMock.Object, mapperMock.Object, cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        GetCountryByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

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
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        cacheManagerMock.Setup(x => x.GetAsync<CountryDto>(request.Id.ToString())).ReturnsAsync(cachedCountry);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(cachedCountry, result);
        repositoryMock.Verify(x => x.FindAsync<CountryAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CountryDoesNotExistInCache_ReturnsCountryFromRepository()
    {
        // Arrange
        var request = new GetCountryByIdQuery(fakeData.Country.Id);
        var countryAggregate = fakeData.CountryAggregate;
        var countryDto = fakeData.Country;
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<CountryAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(countryAggregate);
        mapperMock.Setup(x => x.Map<CountryDto>(countryAggregate)).Returns(countryDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(countryDto, result);
        cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), countryDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CountryNotFoundInRepository_ThrowsCountryNotFoundException()
    {
        // Arrange
        var request = new GetCountryByIdQuery(fakeData.Country.Id);
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<CountryAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((CountryAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.CountryNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CountryNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
