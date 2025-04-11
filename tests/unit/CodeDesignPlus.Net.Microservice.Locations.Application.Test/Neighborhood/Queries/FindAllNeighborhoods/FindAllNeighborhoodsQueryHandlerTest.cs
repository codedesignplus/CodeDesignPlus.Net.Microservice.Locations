using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Queries.FindAllNeighborhoods;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Neighborhood.Queries.FindAllNeighborhoods;

public class FindAllNeighborhoodsQueryHandlerTest
{
    private readonly Mock<INeighborhoodRepository> repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindAllNeighborhoodsQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindAllNeighborhoodsQueryHandlerTest()
    {
        repositoryMock = new Mock<INeighborhoodRepository>();
        _mapperMock = new Mock<IMapper>();
        handler = new FindAllNeighborhoodsQueryHandler(repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindAllNeighborhoodsQuery request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsNeighborhoods()
    {
        // Arrange
        var request = new FindAllNeighborhoodsQuery(null!);
        var cancellationToken = CancellationToken.None;
        var neighborhoods = new List<NeighborhoodAggregate> { fakeData.NeighborhoodAggregate };
        var neighborhoodDtos = new List<NeighborhoodDto> { fakeData.Neighborhood };

        var pagination = Pagination<NeighborhoodAggregate>.Create(neighborhoods, neighborhoods.Count, 10, 0);

        repositoryMock
            .Setup(repo => repo.MatchingAsync<NeighborhoodAggregate>(request.Criteria, cancellationToken))
            .ReturnsAsync(pagination);
        _mapperMock
            .Setup(mapper => mapper.Map<Pagination<NeighborhoodDto>>(pagination))
            .Returns(Pagination<NeighborhoodDto>.Create(neighborhoodDtos, neighborhoodDtos.Count, 10, 0));

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(neighborhoodDtos, result.Data);
    }
}
