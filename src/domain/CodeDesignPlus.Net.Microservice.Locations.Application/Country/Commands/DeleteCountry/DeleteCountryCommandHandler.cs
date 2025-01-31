namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.DeleteCountry;

public class DeleteCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<DeleteCountryCommand>
{
    public Task Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}