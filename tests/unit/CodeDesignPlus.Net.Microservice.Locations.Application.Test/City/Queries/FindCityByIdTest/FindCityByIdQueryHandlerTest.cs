using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindCityById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Queries.FindCityByIdTest;

public class FindCityByIdQueryHandlerTest
{
    private readonly Mock<ICityRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ICacheManager> cacheManagerMock;
    private readonly FindCityByIdQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindCityByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<ICityRepository>();
        mapperMock = new Mock<IMapper>();
        cacheManagerMock = new Mock<ICacheManager>();
        handler = new FindCityByIdQueryHandler(repositoryMock.Object, mapperMock.Object, cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindCityByIdQuery request = null!;

        // Act & Assert
        await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CityExistsInCache_ReturnsCityFromCache()
    {
        // Arrange
        var request = new FindCityByIdQuery(fakeData.City.Id);
        var cityDto = fakeData.City;
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        cacheManagerMock.Setup(x => x.GetAsync<CityDto>(request.Id.ToString())).ReturnsAsync(cityDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(cityDto, result);
        repositoryMock.Verify(x => x.FindAsync<CityAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CityDoesNotExistInCache_ReturnsCityFromRepository()
    {
        // Arrange
        var request = new FindCityByIdQuery(Guid.NewGuid());
        var city = fakeData.CityAggregate;
        var cityDto = fakeData.City;
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<CityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(city);
        mapperMock.Setup(x => x.Map<CityDto>(city)).Returns(cityDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(cityDto, result);
        cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), cityDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CityNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new FindCityByIdQuery(Guid.NewGuid());
        cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<CityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((CityAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.CityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.CityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
