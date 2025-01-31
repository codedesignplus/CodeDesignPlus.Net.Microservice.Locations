namespace CodeDesignPlus.Net.Microservice.Locations.Application.Currency.Commands.UpdateCurrency;

public class UpdateCurrencyCommandHandler(ICurrencyRepository repository, IUserContext user, IPubSub pubsub) : IRequestHandler<UpdateCurrencyCommand>
{
    public Task Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}