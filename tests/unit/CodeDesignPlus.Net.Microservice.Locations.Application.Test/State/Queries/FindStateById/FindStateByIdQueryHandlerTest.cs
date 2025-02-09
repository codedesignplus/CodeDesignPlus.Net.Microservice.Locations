
using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Queries.FindStateById
{
    public class FindStateByIdQueryHandlerTest
    {
        private readonly Mock<IStateRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICacheManager> _cacheManagerMock;
        private readonly FindStateByIdQueryHandler _handler;
        private readonly FakeData fakeData = new();

        public FindStateByIdQueryHandlerTest()
        {
            _repositoryMock = new Mock<IStateRepository>();
            _mapperMock = new Mock<IMapper>();
            _cacheManagerMock = new Mock<ICacheManager>();
            _handler = new FindStateByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object, _cacheManagerMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
        {
            // Arrange
            FindStateByIdQuery request = null!;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_StateExistsInCache_ReturnsStateFromCache()
        {
            // Arrange
            var request = new FindStateByIdQuery(fakeData.State.Id);
            var stateDto = fakeData.State;
            _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
            _cacheManagerMock.Setup(x => x.GetAsync<StateDto>(request.Id.ToString())).ReturnsAsync(stateDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(stateDto, result);
            _repositoryMock.Verify(x => x.FindAsync<StateAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_StateDoesNotExistInCache_ReturnsStateFromRepository()
        {
            // Arrange
            var request = new FindStateByIdQuery(fakeData.State.Id);
            var stateAggregate = fakeData.StateAggregate;
            var stateDto = fakeData.State;
            _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
            _repositoryMock.Setup(x => x.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(stateAggregate);
            _mapperMock.Setup(x => x.Map<StateDto>(stateAggregate)).Returns(stateDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(stateDto, result);
            _cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), stateDto, It.IsAny<TimeSpan?>()), Times.Once);
        }

        [Fact]
        public async Task Handle_StateNotFoundInRepository_ThrowsStateNotFoundException()
        {
            // Arrange
            var request = new FindStateByIdQuery(fakeData.State.Id);
            _cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
            _repositoryMock.Setup(x => x.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((StateAggregate)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, CancellationToken.None));

            Assert.Equal(Errors.StateNotFound.GetMessage(), exception.Message);
            Assert.Equal(Errors.StateNotFound.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }
    }
}
