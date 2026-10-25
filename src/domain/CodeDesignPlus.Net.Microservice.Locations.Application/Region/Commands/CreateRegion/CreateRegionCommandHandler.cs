namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.CreateRegion;

public class CreateRegionCommandHandler(IRegionRepository repository, IUserContext user, ICacheManager cache) : IRequestHandler<CreateRegionCommand>
{
    public async Task Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        // Ya guardado por un envío anterior del mismo formulario: el reintento termina bien sin repetir nada (pendings/589).
        if (await repository.ExistsAsync<RegionAggregate>(request.Id, cancellationToken))
            return;

        var region = RegionAggregate.Create(request.Id, request.Name, request.SubRegions, request.IsActive, user.IdUser);

        // Dos envíos que llegan a la vez pasan los dos la comprobación de arriba; el segundo choca contra el id y no publica.
        if (!await repository.CreateIfAbsentAsync(region, cancellationToken))
            return;

        await cache.RemoveAsync(CacheKeys.ById(region.Id));
    }
}