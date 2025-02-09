namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.UpdateLocality;

public class UpdateLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateLocalityCommand>
{
    public async Task Handle(UpdateLocalityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<LocalityAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.LocalityNotFound);

        var existCity = await repository.ExistsAsync<CityAggregate>(request.IdCity,  cancellationToken);

        ApplicationGuard.IsFalse(existCity, Errors.CityNotFound);

        aggregate.Update(request.IdCity, request.Name, request.IsActive, user.IdUser);

        await repository.UpdateAsync(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}