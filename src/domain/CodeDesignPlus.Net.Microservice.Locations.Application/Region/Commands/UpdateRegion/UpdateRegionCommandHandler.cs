namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.UpdateRegion;

public class UpdateRegionCommandHandler(IRegionRepository repository, IUserContext user) : IRequestHandler<UpdateRegionCommand>
{
    public async Task Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<RegionAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.RegionNotFound);

        aggregate.Update(request.Name, request.SubRegions, request.IsActive, user.IdUser);

        await repository.UpdateAsync(aggregate, cancellationToken);
    }
}