namespace CodeDesignPlus.Net.Microservice.Locations.Application.Locality.Commands.CreateLocality;

public class CreateLocalityCommandHandler(ILocalityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateLocalityCommand>
{
    public async Task Handle(CreateLocalityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<LocalityAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.LocalityAlreadyExists);

        var aggregate = LocalityAggregate.Create(request.Id, request.IdCity, request.Name, user.IdUser);

        var cityExist = await repository.ExistsAsync<CityAggregate>(request.IdCity, cancellationToken);

        ApplicationGuard.IsFalse(cityExist, Errors.CityNotFound);

        await repository.CreateAsync(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}