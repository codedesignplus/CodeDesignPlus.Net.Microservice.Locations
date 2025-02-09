namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

public class CreateCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCountryCommand>
{
    public async Task Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<CountryAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.CountryAlreadyExists);

        var existCurrency = await repository.ExistsAsync<CurrencyAggregate>(request.IdCurrency,  cancellationToken);

        ApplicationGuard.IsFalse(existCurrency, Errors.CurrencyNotFound);

        var country = CountryAggregate.Create(request.Id, request.Name, request.Alpha2, request.Alpha3, request.Code, request.Capital, request.IdCurrency, request.TimeZone, user.IdUser);

        await repository.CreateAsync(country, cancellationToken);

        await pubsub.PublishAsync(country.GetAndClearEvents(), cancellationToken);
    }
}