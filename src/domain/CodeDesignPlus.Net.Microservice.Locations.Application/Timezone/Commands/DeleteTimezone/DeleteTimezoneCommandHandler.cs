namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.DeleteTimezone;

public class DeleteTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteTimezoneCommand>
{
    public Task Handle(DeleteTimezoneCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}