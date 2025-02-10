
using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.State.Queries.FindStateById
{
    public class FindStateByIdQueryHandlerTest
    {
        private readonly Mock<IStateRepository> repositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<ICacheManager> cacheManagerMock;
        private readonly FindStateByIdQueryHandler handler;
        private readonly FakeData fakeData = new();

        public FindStateByIdQueryHandlerTest()
        {
            repositoryMock = new Mock<IStateRepository>();
            mapperMock = new Mock<IMapper>();
            cacheManagerMock = new Mock<ICacheManager>();
            handler = new FindStateByIdQueryHandler(repositoryMock.Object, mapperMock.Object, cacheManagerMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
        {
            // Arrange
            FindStateByIdQuery request = null!;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

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
            cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(true);
            cacheManagerMock.Setup(x => x.GetAsync<StateDto>(request.Id.ToString())).ReturnsAsync(stateDto);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(stateDto, result);
            repositoryMock.Verify(x => x.FindAsync<StateAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_StateDoesNotExistInCache_ReturnsStateFromRepository()
        {
            // Arrange
            var request = new FindStateByIdQuery(fakeData.State.Id);
            var stateAggregate = fakeData.StateAggregate;
            var stateDto = fakeData.State;
            cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
            repositoryMock.Setup(x => x.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(stateAggregate);
            mapperMock.Setup(x => x.Map<StateDto>(stateAggregate)).Returns(stateDto);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(stateDto, result);
            cacheManagerMock.Verify(x => x.SetAsync(request.Id.ToString(), stateDto, It.IsAny<TimeSpan?>()), Times.Once);
        }

        [Fact]
        public async Task Handle_StateNotFoundInRepository_ThrowsStateNotFoundException()
        {
            // Arrange
            var request = new FindStateByIdQuery(fakeData.State.Id);
            cacheManagerMock.Setup(x => x.ExistsAsync(request.Id.ToString())).ReturnsAsync(false);
            repositoryMock.Setup(x => x.FindAsync<StateAggregate>(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((StateAggregate)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

            Assert.Equal(Errors.StateNotFound.GetMessage(), exception.Message);
            Assert.Equal(Errors.StateNotFound.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }
    }
}
