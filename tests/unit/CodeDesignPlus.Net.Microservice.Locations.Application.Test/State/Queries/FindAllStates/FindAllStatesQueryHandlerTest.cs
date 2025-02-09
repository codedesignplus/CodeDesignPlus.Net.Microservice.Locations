using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Queries.FindAllStates;

public class FindAllStatesQueryHandlerTest
{
    private readonly Mock<IStateRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FindAllStatesQueryHandler _handler;
    private readonly FakeData fakeData = new();

    public FindAllStatesQueryHandlerTest()
    {
        _repositoryMock = new Mock<IStateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new FindAllStatesQueryHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        FindAllStatesQuery request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsMappedStates()
    {
        // Arrange
        var request = new FindAllStatesQuery(null!);
        var cancellationToken = CancellationToken.None;
        var stateAggregates = new List<StateAggregate> { fakeData.StateAggregate };
        var stateDtos = new List<StateDto> { fakeData.State };

        _repositoryMock
            .Setup(repo => repo.MatchingAsync<StateAggregate>(request.Criteria, cancellationToken))
            .ReturnsAsync(stateAggregates);
        _mapperMock
            .Setup(mapper => mapper.Map<List<StateDto>>(stateAggregates))
            .Returns(stateDtos);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.Equal(stateDtos, result);
    }
}
