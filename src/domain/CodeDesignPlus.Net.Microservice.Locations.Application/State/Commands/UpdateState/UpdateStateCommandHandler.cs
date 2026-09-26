namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.UpdateState;

public class UpdateStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<UpdateStateCommand>
{
    public async Task Handle(UpdateStateCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<StateAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.StateNotFound);

        var existCountry = await repository.ExistsAsync<CountryAggregate>(request.IdCountry,  cancellationToken);

        ApplicationGuard.IsFalse(existCountry, Errors.CountryNotFound);

        aggregate.Update(request.IdCountry, request.Code, request.Name, request.IsActive, user.IdUser);

        await repository.UpdateAsync(aggregate, cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}