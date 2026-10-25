namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;

public class CreateTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateTimezoneCommand>
{
    public async Task Handle(CreateTimezoneCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<TimezoneAggregate>(request.Id, cancellationToken))
            return;

        var aggregate = TimezoneAggregate.Create(request.Id, request.Name, request.Aliases, request.Location, request.Offsets, request.CurrentOffset, request.IsActive, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(aggregate, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}