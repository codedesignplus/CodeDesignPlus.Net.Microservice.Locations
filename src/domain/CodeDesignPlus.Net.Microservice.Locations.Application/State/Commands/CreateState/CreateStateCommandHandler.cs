namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

public class CreateStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateStateCommand>
{
    public async Task Handle(CreateStateCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<StateAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.StateAlreadyExists);

        var state = StateAggregate.Create(request.Id, request.IdCountry, request.Code, request.Name, user.IdUser);

        await repository.CreateAsync(state, cancellationToken);

        await pubsub.PublishAsync(state.GetAndClearEvents(), cancellationToken);
    }
}