namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public record FindAllStatesQuery(Guid Id) : IRequest<StateDto>;

