namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;

public class UpdateCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateCountryCommand>
{
    public async Task Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CountryAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CountryNotFound);

        var existCurrency = await repository.ExistsAsync<CurrencyAggregate>(request.IdCurrency,  cancellationToken);

        ApplicationGuard.IsFalse(existCurrency, Errors.CurrencyNotFound);

        aggregate.Update(request.Name, request.Alpha2, request.Alpha3, request.Code, request.Capital, request.IdCurrency, request.Timezone, request.NameNative, request.Region, request.SubRegion, request.Latitude, request.Longitude, request.Flag, request.IsActive, user.IdUser);

        await repository.UpdateAsync(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}