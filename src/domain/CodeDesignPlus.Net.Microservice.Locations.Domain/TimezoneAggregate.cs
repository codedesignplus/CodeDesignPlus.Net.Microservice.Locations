using CodeDesignPlus.Net.Microservice.Locations.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class TimezoneAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = null!;
    public List<string> Aliases { get; set; } = [];
    public Location Location { get; set; } = null!;
    public List<string> Offsets { get; set; } = [];
    public string CurrentOffset { get; set; }= null!;

    private TimezoneAggregate(Guid id, string name, List<string> aliases, Location location, List<string> offsets, string currentOffset, bool isActive, Guid createdBy) : this(id)
    {
        this.Name = name;
        this.Aliases = aliases;
        this.Location = location;
        this.Offsets = offsets;
        this.CurrentOffset = currentOffset;
        this.IsActive = isActive;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(TimezoneCreatedDomainEvent.Create(Id, Name, Aliases, Location, Offsets, CurrentOffset, IsActive));
    }

    public static TimezoneAggregate Create(Guid id, string name, List<string> aliases, Location location, List<string> offsets, string currentOffset, bool isActive, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNull(location, Errors.LocationIsInvalid);
        DomainGuard.IsEmpty(offsets, Errors.OffsetsCanNotBeEmpty);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(currentOffset, Errors.CurrentOffsetIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new TimezoneAggregate(id, name, aliases, location, offsets, currentOffset, isActive, createdBy);
    }

    public void Update(string name, List<string> aliases, Location location, List<string> offsets, string currentOffset, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNull(location, Errors.LocationIsInvalid);
        DomainGuard.IsEmpty(offsets, Errors.OffsetsCanNotBeEmpty);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.IsNullOrEmpty(currentOffset, Errors.CurrentOffsetIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);


        this.Name = name;
        this.Aliases = aliases;
        this.Location = location;
        this.Offsets = offsets;
        this.CurrentOffset = currentOffset;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(TimezoneUpdatedDomainEvent.Create(Id, Name, Aliases, Location, Offsets, CurrentOffset, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(TimezoneDeletedDomainEvent.Create(Id, Name, Aliases, Location, Offsets, CurrentOffset, IsActive));
    }


}
