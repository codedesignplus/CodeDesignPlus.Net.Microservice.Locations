namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.DeleteLocality;

public class DeleteLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteLocalityCommand>
{
    public async Task Handle(DeleteLocalityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<LocalityAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.LocalityNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<LocalityAggregate>(aggregate.Id, user.Tenant, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}