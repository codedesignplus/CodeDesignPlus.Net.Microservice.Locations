using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Queries.FindAllLocalities;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Locality.Queries.FindAllLocalities
{
    public class FindAllLocalitiesQueryHandlerTest
    {
        private readonly Mock<ILocalityRepository> repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FindAllLocalitiesQueryHandler handler;
        private readonly FakeData fakeData = new();

        public FindAllLocalitiesQueryHandlerTest()
        {
            repositoryMock = new Mock<ILocalityRepository>();
            _mapperMock = new Mock<IMapper>();
            handler = new FindAllLocalitiesQueryHandler(repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
        {
            // Arrange
            FindAllLocalitiesQuery request = null!;
            var cancellationToken = CancellationToken.None;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

            Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
            Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
            Assert.Equal(Layer.Application, exception.Layer);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsLocalityDtoList()
        {
            // Arrange
            var request = new FindAllLocalitiesQuery(new C.Criteria
            {
                Filters = $"IdCity={fakeData.City.IdState}"
            });
            var cancellationToken = CancellationToken.None;
            var localities = new List<LocalityAggregate> { fakeData.LocalityAggregate };
            var localityDtos = new List<LocalityDto> { fakeData.Locality };

            var pagination = Pagination<LocalityAggregate>.Create(localities, localities.Count, 10, 0);

            repositoryMock
                .Setup(repo => repo.MatchingAsync<LocalityAggregate>(request.Criteria, cancellationToken))
                .ReturnsAsync(pagination);
            _mapperMock.Setup(mapper => mapper.Map<Pagination<LocalityDto>>(pagination))
                .Returns(Pagination<LocalityDto>.Create(localityDtos, localityDtos.Count, 10, 0));

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localityDtos, result.Data);
        }
    }
}
