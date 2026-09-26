namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.DeleteRegion;

/// <summary>
/// Borra una región. No existía: la pantalla mostraba la papelera y el API respondía 405 (plan 044 de pendings).
/// Los países guardan la región por nombre, así que no se borra si algún país la usa (plan 043).
/// </summary>
public class DeleteRegionCommandHandler(IRegionRepository repository, IUserContext user, ICountryRepository countries, ICacheManager cache) : IRequestHandler<DeleteRegionCommand>
{
    public async Task Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<RegionAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.RegionNotFound);

        ApplicationGuard.IsTrue(await countries.AnyByRegionAsync(aggregate.Name, cancellationToken), Errors.RegionIsInUse);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<RegionAggregate>(aggregate.Id, cancellationToken);

        await cache.RemoveAsync(CacheKeys.ById(aggregate.Id));
    }
}
