namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.CreateTimezone;

public class CreateTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateTimezoneCommand>
{
    public Task Handle(CreateTimezoneCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}