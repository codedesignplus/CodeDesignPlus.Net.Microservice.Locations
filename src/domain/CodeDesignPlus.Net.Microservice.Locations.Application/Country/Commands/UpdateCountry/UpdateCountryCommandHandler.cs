namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Commands.UpdateCountry;

public class UpdateCountryCommandHandler(ICountryRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateCountryCommand>
{
    public Task Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}