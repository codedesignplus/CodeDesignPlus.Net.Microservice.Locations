namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.CreateCurrency;

public class CreateCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<CreateCurrencyCommand>
{
    public Task Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}