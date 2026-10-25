namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

public class CreateCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateCountryCommand>
{
    public async Task Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<CountryAggregate>(request.Id, cancellationToken))
            return;

        var existCurrency = await repository.ExistsAsync<CurrencyAggregate>(request.IdCurrency,  cancellationToken);

        ApplicationGuard.IsFalse(existCurrency, Errors.CurrencyNotFound);

        var country = CountryAggregate.Create(request.Id, request.Name, request.Alpha2, request.Alpha3, request.Code, request.PhoneCode, request.Capital, request.IdCurrency, request.Timezone, request.NameNative, request.Region, request.SubRegion, request.Latitude, request.Longitude, request.Flag, request.IsActive, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(country, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.CountryById(country.Id));


        await pubsub.PublishAsync(country.GetAndClearEvents(), cancellationToken);
    }
}