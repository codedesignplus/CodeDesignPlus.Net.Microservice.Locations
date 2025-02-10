using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Queries.FindTimezoneById;

public class FindTimezoneByIdQueryHandlerTest
{
    private readonly Mock<ITimezoneRepository> repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheManager> _cacheManagerMock;
    private readonly FindTimezoneByIdQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindTimezoneByIdQueryHandlerTest()
    {
        repositoryMock = new Mock<ITimezoneRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheManagerMock = new Mock<ICacheManager>();
        handler = new FindTimezoneByIdQueryHandler(repositoryMock.Object, _mapperMock.Object, _cacheManagerMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindTimezoneByIdQuery request = null!;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_TimezoneExistsInCache_ReturnsTimezoneFromCache()
    {
        // Arrange
        var request = new FindTimezoneByIdQuery(fakeData.Timezone.Id);
        var cachedTimezone = fakeData.Timezone;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
        _cacheManagerMock.Setup(x => x.GetAsync<TimezoneDto>(request.Id.ToString())).ReturnsAsync(cachedTimezone);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(cachedTimezone, result);
        repositoryMock.Verify(x => x.FindAsync<TimezoneAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_TimezoneNotInCache_ReturnsTimezoneFromRepository()
    {
        // Arrange
        var request = new FindTimezoneByIdQuery(fakeData.Timezone.Id);
        var timezone = fakeData.TimezoneAggregate;
        var timezoneDto = fakeData.Timezone;
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<TimezoneAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(timezone);
        _mapperMock.Setup(x => x.Map<TimezoneDto>(timezone)).Returns(timezoneDto);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(timezoneDto, result);
        _cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), timezoneDto, It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TimezoneNotFound_ThrowsCodeDesignPlusException()
    {
        // Arrange
        var request = new FindTimezoneByIdQuery(fakeData.Timezone.Id);
        _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.FindAsync<TimezoneAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((TimezoneAggregate)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        Assert.Equal(Errors.TimezoneNotFound.GetMessage(), exception.Message);
        Assert.Equal(Errors.TimezoneNotFound.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }
}
