namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.DeleteState;

public class DeleteStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteStateCommand>
{
    public async Task Handle(DeleteStateCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<StateAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.StateNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<StateAggregate>(aggregate.Id, user.Tenant, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}