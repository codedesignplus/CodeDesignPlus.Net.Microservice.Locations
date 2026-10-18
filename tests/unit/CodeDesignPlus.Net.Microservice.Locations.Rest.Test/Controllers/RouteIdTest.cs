using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;
using CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;
using CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.UpdateNeighborhood;
using CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.UpdateRegion;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

namespace CodeDesignPlus.Net.Microservice.Locations.Rest.Test.Controllers;

/// <summary>
/// An update acts on the id in the route, not on the one in the body (pendings/404): six controllers forwarded the
/// body as is, so a <c>PUT /A</c> with <c>{ id: B }</c> updated B and a body without id failed validation.
/// </summary>
public class RouteIdTest
{
    private readonly Mock<IMediator> mediator = new();
    private readonly IMapper mapper = new Mapper();
    private readonly Guid routeId = Guid.NewGuid();
    private object? Sent => mediator.Invocations.Single().Arguments[0];

    [Fact]
    public async Task UpdateCity_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new CityController(mediator.Object, mapper).UpdateCity(routeId, new UpdateCityDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateCityCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateCountry_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new CountryController(mediator.Object, mapper).UpdateCountry(routeId, new UpdateCountryDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateCountryCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateCurrency_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new CurrencyController(mediator.Object, mapper).UpdateCurrency(routeId, new UpdateCurrencyDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateCurrencyCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateLocality_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new LocalityController(mediator.Object, mapper).UpdateLocality(routeId, new UpdateLocalityDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateLocalityCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateNeighborhood_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new NeighborhoodController(mediator.Object, mapper).UpdateNeighborhood(routeId, new UpdateNeighborhoodDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateNeighborhoodCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateRegion_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new RegionController(mediator.Object, mapper).UpdateRegion(routeId, new UpdateRegionDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateRegionCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateState_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new StateController(mediator.Object, mapper).UpdateState(routeId, new UpdateStateDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateStateCommand>(Sent).Id);
    }

    [Fact]
    public async Task UpdateTimezone_BodyWithAnotherId_ActsOnRouteId()
    {
        // Act
        await new TimezoneController(mediator.Object, mapper).UpdateTimezone(routeId, new UpdateTimezoneDto { Id = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        Assert.Equal(routeId, Assert.IsType<UpdateTimezoneCommand>(Sent).Id);
    }
}
