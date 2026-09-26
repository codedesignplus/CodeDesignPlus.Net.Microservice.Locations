using System.Threading;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;
using CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.DeleteRegion;
using CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;
using CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;
using CodeDesignPlus.Net.Microservice.Locations.Application.Test.Helpers;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.Test.Rules;

/// <summary>
/// Las reglas que salieron del recorrido de Ubicaciones en producción (planes 037, 041, 043 y 044 de pendings).
/// </summary>
public class LocationRulesTest
{
    private readonly FakeData fakeData = new();
    private readonly Mock<IUserContext> user = new();
    private readonly Mock<IPubSub> pubsub = new();
    private readonly Mock<ICacheManager> cache = new();

    public LocationRulesTest()
    {
        user.SetupGet(x => x.IdUser).Returns(Guid.NewGuid());
    }

    [Fact]
    public async Task UpdateCurrency_RemovesTheDetailFromTheCache()
    {
        // Plan 037: sin esto, un cambio de dígitos decimales tardaba hasta 6 h en llegar al gRPC de conversión.
        var request = fakeData.UpdateCurrencyCommand;
        var repository = new Mock<ICurrencyRepository>();
        repository.Setup(r => r.FindAsync<CurrencyAggregate>(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CurrencyAggregate.Create(request.Id, "XTS", 963, 2, "¤", "Prueba", Guid.NewGuid()));

        var handler = new UpdateCurrencyCommandHandler(repository.Object, user.Object, pubsub.Object, cache.Object);

        await handler.Handle(request, CancellationToken.None);

        cache.Verify(c => c.RemoveAsync(CacheKeys.CurrencyById(request.Id)), Times.Once);
    }

    [Fact]
    public async Task DeleteState_WithCities_IsRejected_AndNothingIsDeleted()
    {
        // Plan 043: borrar un estado con ciudades dejaba las ciudades huérfanas.
        var id = Guid.NewGuid();
        var repository = new Mock<IStateRepository>();
        var cities = new Mock<ICityRepository>();
        repository.Setup(r => r.FindAsync<StateAggregate>(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(StateAggregate.Create(id, Guid.NewGuid(), "99", "PRUEBA", Guid.NewGuid()));
        cities.Setup(r => r.AnyByStateAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteStateCommandHandler(repository.Object, user.Object, pubsub.Object, cities.Object, cache.Object);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(new DeleteStateCommand(id), CancellationToken.None));

        Assert.Equal(Errors.StateHasCities.GetCode(), exception.Code);
        repository.Verify(r => r.DeleteAsync<StateAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteState_WithoutCities_IsDeleted()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IStateRepository>();
        var cities = new Mock<ICityRepository>();
        repository.Setup(r => r.FindAsync<StateAggregate>(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(StateAggregate.Create(id, Guid.NewGuid(), "99", "PRUEBA", Guid.NewGuid()));

        var handler = new DeleteStateCommandHandler(repository.Object, user.Object, pubsub.Object, cities.Object, cache.Object);

        await handler.Handle(new DeleteStateCommand(id), CancellationToken.None);

        repository.Verify(r => r.DeleteAsync<StateAggregate>(id, It.IsAny<CancellationToken>()), Times.Once);
        cache.Verify(c => c.RemoveAsync(CacheKeys.ById(id)), Times.Once);
    }

    [Fact]
    public async Task DeleteRegion_UsedByACountry_IsRejected()
    {
        // Planes 043 y 044: la región no se podía borrar; ahora sí, salvo que un país la use (se guarda por nombre).
        var id = Guid.NewGuid();
        var repository = new Mock<IRegionRepository>();
        var countries = new Mock<ICountryRepository>();
        repository.Setup(r => r.FindAsync<RegionAggregate>(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(RegionAggregate.Create(id, "América", ["Caribe"], true, Guid.NewGuid()));
        countries.Setup(r => r.AnyByRegionAsync("América", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var handler = new DeleteRegionCommandHandler(repository.Object, user.Object, countries.Object, cache.Object);

        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(new DeleteRegionCommand(id), CancellationToken.None));

        Assert.Equal(Errors.RegionIsInUse.GetCode(), exception.Code);
        repository.Verify(r => r.DeleteAsync<RegionAggregate>(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteRegion_Unused_IsDeleted()
    {
        var id = Guid.NewGuid();
        var repository = new Mock<IRegionRepository>();
        var countries = new Mock<ICountryRepository>();
        repository.Setup(r => r.FindAsync<RegionAggregate>(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(RegionAggregate.Create(id, "Prueba Guía", ["Subregión A"], true, Guid.NewGuid()));

        var handler = new DeleteRegionCommandHandler(repository.Object, user.Object, countries.Object, cache.Object);

        await handler.Handle(new DeleteRegionCommand(id), CancellationToken.None);

        repository.Verify(r => r.DeleteAsync<RegionAggregate>(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void CreateTimezone_WithoutLocation_ReportsTheErrorOnce()
    {
        // Plan 041: «Location es obligatorio.» salía dos veces porque se encadenaban NotEmpty y NotNull.
        var command = new CreateTimezoneCommand(Guid.NewGuid(), "Prueba/Guia", [], null!, ["-05:00"], "-05:00", true);

        var result = new CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone.Validator().TestValidate(command);

        Assert.Single(result.Errors, e => e.PropertyName == nameof(CreateTimezoneCommand.Location));
    }
}
