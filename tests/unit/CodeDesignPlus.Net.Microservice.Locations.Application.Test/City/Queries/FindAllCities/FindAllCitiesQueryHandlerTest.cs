using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Queries.FindAllCities;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.City.Queries.FindAllCities
{
    public class FindAllCitiesQueryHandlerTest
    {
        private readonly Mock<ICityRepository> repositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly FindAllCitiesQueryHandler handler;
        private readonly FakeData fakeData = new();

        public FindAllCitiesQueryHandlerTest()
        {
            repositoryMock = new Mock<ICityRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new FindAllCitiesQueryHandler(repositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            // Arrange
            FindAllCitiesQuery request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsCityDtoList()
        {
            // Arrange
            var request = new FindAllCitiesQuery(new C.Criteria
            {
                Filters = $"IdState={fakeData.City.IdState}"
            });
            var cancellationToken = CancellationToken.None;
            var cityAggregates = new List<CityAggregate> { fakeData.CityAggregate };
            var cityDtos = new List<CityDto> { fakeData.City };

            var pagination = Pagination<CityAggregate>.Create(cityAggregates, cityAggregates.Count, 10, 0);

            repositoryMock
                .Setup(repo => repo.MatchingAsync<CityAggregate>(request.Criteria, cancellationToken))
                .ReturnsAsync(pagination);
            mapperMock
                .Setup(mapper => mapper.Map<Pagination<CityDto>>(pagination))
                .Returns(Pagination<CityDto>.Create(cityDtos, cityDtos.Count, 10, 0));

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cityDtos, result.Data);
        }
    }
}
