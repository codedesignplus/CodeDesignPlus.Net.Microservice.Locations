using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindLocalityById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Queries.FindLocalityById;

public class FindLocalityByIdQueryHandlerTest
{
    private readonly Mock<ILocalityRepository> repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheManager> _cacheManagerMock;
    private readonly FindLocalityByIdQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindLocalityByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<ILocalityRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheManagerMock = new Mock<ICacheManager>();
        handler = new FindLocalityByIdQueryHandler(repositoryMock.Object, _mapperMock.Object, _cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindLocalityByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_LocalityExistsInCache_ReturnsLocalityFromCache()
    {
        // Arrange
        var request = new FindLocalityByIdQuery(fakeData.Locality.Id);
        var localityDto = fakeData.Locality;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        _cacheManagerMock.Setup(x => x.GetAsync<LocalityDto>(request.Id.ToString())).ReturnsAsync(localityDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(localityDto, result);
        repositoryMock.Verify(x => x.FindAsync<LocalityAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_LocalityNotInCache_ReturnsLocalityFromRepository()
    {
        // Arrange
        var request = new FindLocalityByIdQuery(Guid.NewGuid());
        var locality = fakeData.LocalityAggregate;
        var localityDto = fakeData.Locality;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(locality);
        _mapperMock.Setup(x => x.Map<LocalityDto>(locality)).Returns(localityDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(localityDto, result);
        _cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), localityDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_LocalityNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new FindLocalityByIdQuery(Guid.NewGuid());
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<LocalityAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((LocalityAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.LocalityNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.LocalityNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
