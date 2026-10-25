namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Commands.CreateState;

public class CreateStateCommandHandler(IStateRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateStateCommand>
{
    public async Task Handle(CreateStateCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<StateAggregate>(request.Id, cancellationToken))
            return;

        var existCountry = await repository.ExistsAsync<CountryAggregate>(request.IdCountry,  cancellationToken);

        ApplicationGuard.IsFalse(existCountry, Errors.CountryNotFound);

        var state = StateAggregate.Create(request.Id, request.IdCountry, request.Code, request.Name, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(state, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.ById(state.Id));

        await pubsub.PublishAsync(state.GetAndClearEvents(), cancellationToken);
    }
}