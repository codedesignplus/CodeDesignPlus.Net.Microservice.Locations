namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;

public class CreateTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateTimezoneCommand>
{
    public async Task Handle(CreateTimezoneCommand request, CancellationToken cancellationToken)
    {        
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var exist = await repository.ExistsAsync<TimezoneAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsTrue(exist, Errors.TimezoneAlreadyExists);

        var aggregate = TimezoneAggregate.Create(request.Id, request.Name, request.Aliases, request.Location, request.Offsets, request.CurrentOffset, request.IsActive, user.IdUser);

        await repository.CreateAsync(aggregate, cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}