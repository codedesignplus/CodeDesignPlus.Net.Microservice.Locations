namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindStateById;

public record FindStateByIdQuery(Guid Id) : IRequest<StateDto>;

