using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;

namespace CodeDesignPlus.Net.Microservice.Locations.Application.State.Queries.FindAllStates;

public record FindAllStatesQuery(C.Criteria Criteria) : IRequest<Pagination<StateDto>>;

