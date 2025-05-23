using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Queries.FindAllStates;

public class FindAllStatesQueryHandlerTest
{
    private readonly Mock<IStateRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly FindAllStatesQueryHandler handler;
    private readonly FakeData fakeData = new();

    public FindAllStatesQueryHandlerTest()
    {
        repositoryMock = new Mock<IStateRepository>();
        mapperMock = new Mock<IMapper>();
        handler = new FindAllStatesQueryHandler(repositoryMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindAllStatesQuery request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsMappedStates()
    {
        // Arrange
        var request = new FindAllStatesQuery(new C.Criteria
        {
            Filters = $"IdCountry={fakeData.City.IdState}"
        });
        var cancellationToken = CancellationToken.None;
        var stateAggregates = new List<StateAggregate> { fakeData.StateAggregate };
        var stateDtos = new List<StateDto> { fakeData.State };

        var pagination = Pagination<StateAggregate>.Create(stateAggregates, stateAggregates.Count, 10, 0);

        repositoryMock
            .Setup(repo => repo.MatchingAsync<StateAggregate>(request.Criteria, cancellationToken))
            .ReturnsAsync(pagination);
        mapperMock
            .Setup(mapper => mapper.Map<Pagination<StateDto>>(pagination))
            .Returns(Pagination<StateDto>.Create(stateDtos, stateDtos.Count, 10, 0));

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.Equal(stateDtos, result.Data);
    }
}
