namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class RegionAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = null!;
    public List<string> SubRegions { get; private set; } = [];


    private RegionAggregate(Guid id, string name, List<string> subRegions, bool isActive, Guid createdBy) : this(id)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.RegionNameIsRequired);
        DomainGuard.IsEmpty(subRegions, Errors.SubRegionsAreRequired);

        this.Name = name;
        this.SubRegions = subRegions;
        this.CreatedBy = createdBy;
        this.IsActive = isActive;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();
    }

    public static RegionAggregate Create(Guid id, string name, List<string> subRegions, bool isActive, Guid createdBy)
    {
        return new RegionAggregate(id, name, subRegions, isActive, createdBy);
    }

    public void Update(string name, List<string> subRegions, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.RegionNameIsRequired);
        DomainGuard.IsEmpty(subRegions, Errors.SubRegionsAreRequired);

        this.Name = name;
        this.SubRegions = subRegions;
        this.IsActive = isActive;
        this.UpdatedBy = updatedBy;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
    }
}
