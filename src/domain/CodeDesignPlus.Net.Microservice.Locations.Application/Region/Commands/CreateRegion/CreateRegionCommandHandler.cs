namespace CodeDesignPlus.Net.Microservice.Locations.Application.Region.Commands.CreateRegion;

public class CreateRegionCommandHandler(IRegionRepository repository, IUserContext user) : IRequestHandler<CreateRegionCommand>
{
    public async Task Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<RegionAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.RegionAlreadyExists);

        var region = RegionAggregate.Create(request.Id, request.Name, request.SubRegions, request.IsActive, user.IdUser);

        await repository.CreateAsync(region, cancellationToken);
    }
}