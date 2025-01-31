namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public record FindAllStatesQuery(C.Criteria Criteria) : IRequest<List<StateDto>>;

