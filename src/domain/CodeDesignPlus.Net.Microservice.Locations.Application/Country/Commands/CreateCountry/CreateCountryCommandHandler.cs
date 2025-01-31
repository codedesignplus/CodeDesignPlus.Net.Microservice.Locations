namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

public class CreateCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCountryCommand>
{
    public async Task Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<CountryAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.CountryAlreadyExists);

        var country = CountryAggregate.Create(request.Id, request.Name, request.Code, request.IdCurrency, request.TimeZone, user.IdUser);

        await repository.CreateAsync(country, cancellationToken);

        await pubsub.PublishAsync(country.GetAndClearEvents(), cancellationToken);
    }
}