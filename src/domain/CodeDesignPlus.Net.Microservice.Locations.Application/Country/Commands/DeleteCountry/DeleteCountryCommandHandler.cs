namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;

public class DeleteCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteCountryCommand>
{
    public async Task Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CountryAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CountryNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<CountryAggregate>(aggregate.Id,  cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}