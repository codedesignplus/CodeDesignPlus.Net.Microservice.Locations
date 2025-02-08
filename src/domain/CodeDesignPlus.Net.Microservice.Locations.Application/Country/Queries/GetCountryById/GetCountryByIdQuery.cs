namespace CodeDesignPlus.Net.Microservice.Locations.Application.Country.Queries.GetCountryById;

public record GetCountryByIdQuery(Guid Id) : IRequest<CountryDto>;

public class Validator : AbstractValidator<GetCountryByIdQuery>
{
    public Validator()
    {
        this.RuleFor(x => x.Id).NotEmpty();
    }
}