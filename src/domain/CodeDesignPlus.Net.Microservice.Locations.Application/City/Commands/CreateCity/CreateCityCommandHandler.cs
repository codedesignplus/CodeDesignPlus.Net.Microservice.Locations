namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

public class CreateCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCityCommand>
{
    public async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<CityAggregate>(request.Id, user.Tenant, cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.CityAlreadyExists);

        var city = CityAggregate.Create(request.Id, request.IdState, request.Name, request.TimeZone,user.IdUser);

        await repository.CreateAsync(city, cancellationToken);

        await pubsub.PublishAsync(city.GetAndClearEvents(), cancellationToken);
    }
}