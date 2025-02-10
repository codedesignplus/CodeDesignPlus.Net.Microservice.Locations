using System;
using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.DeleteCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Domain.DomainEvents;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Currency.Commands.DeleteCurrency
{
    public class DeleteCurrencyCommandHandlerTest
    {
        private readonly Mock<ICurrencyRepository> repositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPubSub> _pubSubMock;
        private readonly DeleteCurrencyCommandHandler handler;
        private readonly FakeData fakeData = new();

        public DeleteCurrencyCommandHandlerTest()
        {
            repositoryMock = new Mock<ICurrencyRepository>();
            _userContextMock = new Mock<IUserContext>();
            _pubSubMock = new Mock<IPubSub>();
            handler = new DeleteCurrencyCommandHandler(repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            // Arrange
            DeleteCurrencyCommand request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_CurrencyNotFound_ThrowsCodeDesignPlusException()
        {
            // Arrange
            var request = fakeData.DeleteCurrencyCommand;
            var cancellationToken = CancellationToken.None;

            repositoryMock
                .Setup(repo => repo.FindAsync<CurrencyAggregate>(request.Id, cancellationToken))
                .ReturnsAsync((CurrencyAggregate)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.CurrencyNotFound.GetMessage(), exception.Message);
            Assert.Equal(Errors.CurrencyNotFound.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_DeletesCurrencyAndPublishesEvents()
        {
            // Arrange
            var request = fakeData.DeleteCurrencyCommand;
            var cancellationToken = CancellationToken.None;
            var aggregate = fakeData.CurrencyAggregate;

            repositoryMock
                .Setup(repo => repo.FindAsync<CurrencyAggregate>(request.Id, cancellationToken))
                .ReturnsAsync(aggregate);

            _userContextMock.SetupGet(user => user.IdUser).Returns(Guid.NewGuid());

            // Act
            await handler.Handle(request, cancellationToken);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync<CurrencyAggregate>(aggregate.Id, cancellationToken), Times.Once);
            _pubSubMock.Verify(pubsub => pubsub.PublishAsync(It.IsAny<List<CurrencyDeletedDomainEvent>>(), cancellationToken), Times.AtMostOnce);
        }
    }
}
