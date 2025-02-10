using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;


namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Timezone.Queries.FindAllTimezones
{
    public class FindAllTimezonesQueryHandlerTest
    {
        private readonly Mock<ITimezoneRepository> repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FindAllTimezonesQueryHandler handler;
        private readonly FakeData fakeData = new();

        public FindAllTimezonesQueryHandlerTest()
        {
            repositoryMock = new Mock<ITimezoneRepository>();
            _mapperMock = new Mock<IMapper>();
            handler = new FindAllTimezonesQueryHandler(repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            // Arrange
            FindAllTimezonesQuery request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsMappedTimezones()
        {
            // Arrange
            var request = new FindAllTimezonesQuery(null!);
            var cancellationToken = CancellationToken.None;
            var timezones = new List<TimezoneAggregate> { fakeData.TimezoneAggregate };
            var timezoneDtos = new List<TimezoneDto> { fakeData.Timezone };

            repositoryMock
                .Setup(repo => repo.MatchingAsync<TimezoneAggregate>(request.Criteria, cancellationToken))
                .ReturnsAsync(timezones);
            _mapperMock
                .Setup(mapper => mapper.Map<List<TimezoneDto>>(timezones))
                .Returns(timezoneDtos);

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.Equal(timezoneDtos, result);
        }
    }
}
