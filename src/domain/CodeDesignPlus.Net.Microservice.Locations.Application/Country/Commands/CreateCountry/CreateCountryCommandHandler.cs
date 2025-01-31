namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.CreateCountry;

public class CreateCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCountryCommand>
{
    public Task Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}