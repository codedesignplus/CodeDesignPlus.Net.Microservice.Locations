using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Commands.DeleteCity
{
    public class DeleteCityCommandHandlerTest
    {
        private readonly Mock<ICityRepository> _repositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPubSub> _pubSubMock;
        private readonly DeleteCityCommandHandler _handler;
        private readonly FakeData utils;

        public DeleteCityCommandHandlerTest()
        {
            utils = new FakeData();
            _repositoryMock = new Mock<ICityRepository>();
            _userContextMock = new Mock<IUserContext>();
            _pubSubMock = new Mock<IPubSub>();
            _handler = new DeleteCityCommandHandler(_repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
        {
            // Arrange
            DeleteCityCommand request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));
        }

        [Fact]
        public async Task Handle_CityNotFound_ThrowsCityNotFoundException()
        {
            // Arrange
            var request = utils.DeleteCityCommand;
            var cancellationToken = CancellationToken.None;

            _repositoryMock.Setup(r => r.FindAsync<CityAggregate>(It.IsAny<Guid>(), cancellationToken))
                .ReturnsAsync((CityAggregate)null!);

            // Act & Assert
            await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidRequest_DeletesCityAndPublishesEvents()
        {
            // Arrange
            var request = utils.DeleteCityCommand;
            var cancellationToken = CancellationToken.None;
            var cityAggregate = utils.CityAggregate;

            _repositoryMock.Setup(r => r.FindAsync<CityAggregate>(It.IsAny<Guid>(), cancellationToken))
                .ReturnsAsync(cityAggregate);

            _userContextMock.Setup(u => u.IdUser).Returns(Guid.NewGuid());

            // Act
            await _handler.Handle(request, cancellationToken);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync<CityAggregate>(cityAggregate.Id, cancellationToken), Times.Once);
            _pubSubMock.Verify(p => p.PublishAsync(It.IsAny<List<CityDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
        }
    }
}
