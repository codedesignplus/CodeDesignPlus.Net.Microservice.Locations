namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindAllTimezones;

public record FindAllTimezonesQuery(Guid Id) : IRequest<TimezoneDto>;

