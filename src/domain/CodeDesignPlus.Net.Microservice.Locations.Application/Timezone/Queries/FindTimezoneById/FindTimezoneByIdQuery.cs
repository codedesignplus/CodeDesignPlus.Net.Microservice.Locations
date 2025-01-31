namespace CodeDesignPlus.Net.Microservice.Locations.Application.Timezone.Queries.FindTimezoneById;

public record FindTimezoneByIdQuery(Guid Id) : IRequest<TimezoneDto>;

