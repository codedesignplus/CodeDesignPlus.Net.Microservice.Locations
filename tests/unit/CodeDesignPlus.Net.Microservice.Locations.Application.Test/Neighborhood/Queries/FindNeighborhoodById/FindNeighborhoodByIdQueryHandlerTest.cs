using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindNeighborhoodById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Queries.FindNeighborhoodById;

public class FindNeighborhoodByIdQueryHandlerTest
{
    private readonly Mock<INeighborhoodRepository> repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheManager> _cacheManagerMock;
    private readonly FindNeighborhoodByIdQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindNeighborhoodByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<INeighborhoodRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheManagerMock = new Mock<ICacheManager>();
        handler = new FindNeighborhoodByIdQueryHandler(repositoryMock.Object, _mapperMock.Object, _cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindNeighborhoodByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NeighborhoodExistsInCache_ReturnsNeighborhoodFromCache()
    {
        // Arrange
        var request = new FindNeighborhoodByIdQuery(fakeData.Neighborhood.Id);
        var neighborhoodDto = fakeData.Neighborhood;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        _cacheManagerMock.Setup(x => x.GetAsync<NeighborhoodDto>(request.Id.ToString())).ReturnsAsync(neighborhoodDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(neighborhoodDto, result);
        repositoryMock.Verify(x => x.FindAsync<NeighborhoodAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_NeighborhoodNotInCache_FindsNeighborhoodInRepository()
    {
        // Arrange
        var request = new FindNeighborhoodByIdQuery(Guid.NewGuid());
        var neighborhood = fakeData.NeighborhoodAggregate;
        var neighborhoodDto = fakeData.Neighborhood;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<NeighborhoodAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(neighborhood);
        _mapperMock.Setup(x => x.Map<NeighborhoodDto>(neighborhood)).Returns(neighborhoodDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(neighborhoodDto, result);
        _cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), neighborhoodDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NeighborhoodNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new FindNeighborhoodByIdQuery(Guid.NewGuid());
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<NeighborhoodAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((NeighborhoodAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.NeighborhoodNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.NeighborhoodNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
