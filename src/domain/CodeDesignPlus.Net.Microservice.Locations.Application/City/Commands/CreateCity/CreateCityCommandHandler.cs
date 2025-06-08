namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.CreateCity;

public class CreateCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCityCommand>
{
    public async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);
        
        var exist = await repository.ExistsAsync<CityAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.CityAlreadyExists);

        var existState = await repository.ExistsAsync<StateAggregate>(request.IdState,  cancellationToken);

        ApplicationGuard.IsFalse(existState, Errors.StateNotFound);

        var city = CityAggregate.Create(request.Id, request.IdState, request.Name, request.Timezone,user.IdUser);

        await repository.CreateAsync(city, cancellationToken);

        await pubsub.PublishAsync(city.GetAndClearEvents(), cancellationToken);
    }
}