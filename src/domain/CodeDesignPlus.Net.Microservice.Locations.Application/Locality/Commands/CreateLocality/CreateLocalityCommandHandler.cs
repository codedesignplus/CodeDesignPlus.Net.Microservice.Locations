namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;

public class CreateLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateLocalityCommand>
{
    public async Task Handle(CreateLocalityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<LocalityAggregate>(request.Id, cancellationToken))
            return;

        var cityExist = await repository.ExistsAsync<CityAggregate>(request.IdCity, cancellationToken);

        ApplicationGuard.IsFalse(cityExist, Errors.CityNotFound);
        
        var aggregate = LocalityAggregate.Create(request.Id, request.IdCity, request.Name, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(aggregate, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}