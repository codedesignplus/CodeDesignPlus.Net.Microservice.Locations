
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetAllCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using Moq;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Country.Queries.GetAllCountry;
public class GetAllCountryQueryHandlerTest
{
    private readonly Mock<ICountryRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly GetAllCountryQueryHandler handler;
    private readonly FakeData fakeData = new();

    public GetAllCountryQueryHandlerTest()
    {
        repositoryMock = new Mock<ICountryRepository>();
        mapperMock = new Mock<IMapper>();
        handler = new GetAllCountryQueryHandler(repositoryMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsCodeDesignPlusException()
    {
        // Arrange
        GetAllCountryQuery request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsCountryDtoList()
    {
        // Arrange
        var request = new GetAllCountryQuery(new C.Criteria { Filters = "Alpha2=CO", Limit = 10 });
        var cancellationToken = CancellationToken.None;
        var countries = new List<CountryAggregate> { fakeData.CountryAggregate };
        var countryDtos = new List<CountryDto> { fakeData.Country };

        var pagination = Pagination<CountryAggregate>.Create(countries, countries.Count, 10, 0);

        repositoryMock
            .Setup(repo => repo.MatchingAsync<CountryAggregate>(request.Criteria, cancellationToken))
            .ReturnsAsync(pagination);
        mapperMock
            .Setup(mapper => mapper.Map<Pagination<CountryDto>>(pagination))
            .Returns(Pagination<CountryDto>.Create(countryDtos, countryDtos.Count, 10, 0));

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(countryDtos, result.Data);
    }

    [Fact]
    public async Task Handle_WithoutFilter_RespectsPageLimitAndOrder()
    {
        // Plan 038: sin filtro devolvía toda la lista desde la caché, ignorando skip, limit y orderBy.
        var criteria = new C.Criteria { Limit = 10, Skip = 0, OrderBy = "name" };
        var page = Pagination<CountryAggregate>.Create([], 0, 10, 0);

        repositoryMock
            .Setup(repo => repo.MatchingAsync<CountryAggregate>(criteria, It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        await handler.Handle(new GetAllCountryQuery(criteria), CancellationToken.None);

        repositoryMock.Verify(repo => repo.MatchingAsync<CountryAggregate>(criteria, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
