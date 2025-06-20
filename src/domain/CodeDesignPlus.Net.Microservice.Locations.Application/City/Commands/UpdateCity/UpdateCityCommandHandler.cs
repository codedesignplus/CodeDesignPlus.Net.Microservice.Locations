namespace CodeDesignPlus.Net.Microservice.Locations.Application.City.Commands.UpdateCity;

public class UpdateCityCommandHandler(ICityRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateCityCommand>
{
    public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var city = await repository.FindAsync<CityAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(city, Errors.CityNotFound);

        var existState = await repository.ExistsAsync<StateAggregate>(request.IdState,  cancellationToken);

        ApplicationGuard.IsFalse(existState, Errors.StateNotFound);

        city.Update(request.IdState, request.Name, request.Timezone, request.IsActive, user.IdUser);

        await repository.UpdateAsync(city, cancellationToken);

        await pubsub.PublishAsync(city.GetAndClearEvents(), cancellationToken);
    }
}