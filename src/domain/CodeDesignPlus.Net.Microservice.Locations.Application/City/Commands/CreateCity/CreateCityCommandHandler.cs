namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

public class CreateCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub, ICacheManager cache) : IRequestHandler<CreateCityCommand>
{
    public async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<CityAggregate>(request.Id, cancellationToken))
            return;

        var existState = await repository.ExistsAsync<StateAggregate>(request.IdState,  cancellationToken);

        ApplicationGuard.IsFalse(existState, Errors.StateNotFound);

        var city = CityAggregate.Create(request.Id, request.IdState, request.Name, request.Timezone,user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(city, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.ById(city.Id));

        await pubsub.PublishAsync(city.GetAndClearEvents(), cancellationToken);
    }
}