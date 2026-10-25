namespace CodeDesignPlus.Net.Microservice.Locations.Application.Neighborhood.Commands.CreateNeighborhood;

public class CreateNeighborhoodCommandHandler(INeighborhoodRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateNeighborhoodCommand>
{
    public async Task Handle(CreateNeighborhoodCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<NeighborhoodAggregate>(request.Id, cancellationToken))
            return;

        var existLocality = await repository.ExistsAsync<LocalityAggregate>(request.IdLocality,  cancellationToken);

        ApplicationGuard.IsFalse(existLocality, Errors.LocalityNotFound);

        var aggregate = NeighborhoodAggregate.Create(request.Id, request.IdLocality, request.Name, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(aggregate, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}