
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
        var request = new GetAllCountryQuery(null!);
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
}
