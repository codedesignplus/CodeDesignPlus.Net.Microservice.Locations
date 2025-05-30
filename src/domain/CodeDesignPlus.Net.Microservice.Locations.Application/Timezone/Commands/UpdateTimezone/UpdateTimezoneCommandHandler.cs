namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

public class UpdateTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateTimezoneCommand>
{
    public async Task Handle(UpdateTimezoneCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var client = await repository.FindAsync<TimezoneAggregate>(request.Id, cancellationToken);

        ApplicationGuard.IsNull(client, Errors.TimezoneNotFound);

        client.Update(request.Name, request.Aliases, request.Location, request.Offsets, request.CurrentOffset, request.IsActive, user.IdUser);

        await repository.UpdateAsync(client, cancellationToken);

        await pubsub.PublishAsync(client.GetAndClearEvents(), cancellationToken);
    }
}