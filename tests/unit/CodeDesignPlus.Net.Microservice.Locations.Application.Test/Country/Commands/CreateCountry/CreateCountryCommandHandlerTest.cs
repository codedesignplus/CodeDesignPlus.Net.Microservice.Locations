using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Commands.CreateCountry;

    public class CreateCountryCommandHandlerTest
    {
        private readonly Mock<ICountryRepository> _repositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IPubSub> _pubSubMock;
        private readonly CreateCountryCommandHandler _handler;
        private readonly FakeData fakeData = new();

        public CreateCountryCommandHandlerTest()
        {
            _repositoryMock = new Mock<ICountryRepository>();
            _userContextMock = new Mock<IUserContext>();
            _pubSubMock = new Mock<IPubSub>();
            _handler = new CreateCountryCommandHandler(_repositoryMock.Object, _userContextMock.Object, _pubSubMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(null!, CancellationToken.None));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        } 

        [Fact]
        public async Task Handle_CountryAlreadyExists_ThrowsCodeDesignPlusException()
        {
            var command = fakeData.CreateCountryCommand;
            _repositoryMock.Setup(x => x.ExistsAsync<CountryAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(Errors.CountryAlreadyExists.GetMessage(), exception.Message);
            Assert.Equal(Errors.CountryAlreadyExists.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_CurrencyNotFound_ThrowsCodeDesignPlusException()
        {
            var command = fakeData.CreateCountryCommand;
            _repositoryMock.Setup(x => x.ExistsAsync<CountryAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _repositoryMock.Setup(x => x.ExistsAsync<CurrencyAggregate>(command.IdCurrency, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(Errors.CurrencyNotFound.GetMessage(), exception.Message);
            Assert.Equal(Errors.CurrencyNotFound.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesCountryAndPublishesEvents()
        {
            var command = fakeData.CreateCountryCommand;

            _repositoryMock.Setup(x => x.ExistsAsync<CountryAggregate>(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _repositoryMock.Setup(x => x.ExistsAsync<CurrencyAggregate>(command.IdCurrency, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _userContextMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());

            await _handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<CountryAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
            _pubSubMock.Verify(x => x.PublishAsync(It.IsAny<List<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.AtMostOnce);
        }
    }
