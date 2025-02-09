using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.UpdateCurrency
{
    public class UpdateCurrencyCommandHandlerTest
    {
        private readonly Mock<ICurrencyRepository> _repositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPubSub> _pubSubMock;
        private readonly UpdateCurrencyCommandHandler _handler;
        private readonly FakeData fakeData = new();

        public UpdateCurrencyCommandHandlerTest()
        {
            _repositoryMock = new Mock<ICurrencyRepository>();
            _userContextMock = new Mock<IUserContext>();
            _pubSubMock = new Mock<IPubSub>();
            _handler = new UpdateCurrencyCommandHandler(_repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            // Arrange
            UpdateCurrencyCommand request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_CurrencyNotFound_ThrowsCodeDesignPlusException()
        {
            // Arrange
            var request = fakeData.UpdateCurrencyCommand;
            var cancellationToken = CancellationToken.None;

            _repositoryMock
                .Setup(repo => repo.FindAsync<CurrencyAggregate>(request.Id, cancellationToken))
                .ReturnsAsync((CurrencyAggregate)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.CurrencyNotFound.GetMessage(), exception.Message);
            Assert.Equal(Errors.CurrencyNotFound.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesCurrencyAndPublishesEvents()
        {
            // Arrange
            var request = fakeData.UpdateCurrencyCommand;
            var cancellationToken = CancellationToken.None;
            var aggregate = fakeData.CurrencyAggregate;

            _repositoryMock
                .Setup(repo => repo.FindAsync<CurrencyAggregate>(request.Id, cancellationToken))
                .ReturnsAsync(aggregate);
            _userContextMock.SetupGet(user => user.IdUser).Returns(Guid.NewGuid());

            // Act
            await _handler.Handle(request, cancellationToken);

            // Assert
            _repositoryMock.Verify(repo => repo.UpdateAsync(aggregate, cancellationToken), Times.Once);
            _pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<CurrencyUpdatedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
        }
    }
}
