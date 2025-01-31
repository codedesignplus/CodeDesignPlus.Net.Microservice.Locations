namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;

public record FindAllTimezonesQuery(C.Criteria Criteria) : IRequest<TimezoneDto>;

