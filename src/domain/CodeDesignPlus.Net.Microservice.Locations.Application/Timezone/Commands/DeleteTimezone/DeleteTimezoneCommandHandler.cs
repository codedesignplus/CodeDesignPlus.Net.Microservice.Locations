namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.DeleteTimezone;

public class DeleteTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteTimezoneCommand>
{
    public async Task Handle(DeleteTimezoneCommand request, CancellationToken cancellationToken)
    {
        ApplicationGuard.IsNull(request, Errors.InvalidRequest);

        var aggregate = await repository.FindAsync<TimezoneAggregate>(request.Id,  cancellationToken);

        ApplicationGuard.IsNull(aggregate, Errors.TimezoneNotFound);

        aggregate.Delete(user.IdUser);

        await repository.DeleteAsync<TimezoneAggregate>(aggregate.Id,  cancellationToken);

        await pubsub.PublishAsync(aggregate.GetAndClearEvents(), cancellationToken);
    }
}