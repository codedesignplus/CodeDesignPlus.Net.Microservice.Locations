namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.DeleteCity;

public class DeleteCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteCityCommand>
{
    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<CityAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.CityNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<CityAggregate>(aggregate.Id, user.Tenant, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}