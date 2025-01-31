namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Commands.UpdateTimezone;

public class UpdateTimezoneCommandHandler(ITimezoneRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateTimezoneCommand>
{
    public Task Handle(UpdateTimezoneCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}