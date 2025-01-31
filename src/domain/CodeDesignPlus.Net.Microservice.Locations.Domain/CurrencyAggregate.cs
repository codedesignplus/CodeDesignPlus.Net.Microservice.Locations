namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CurrencyAggregate(Guid id) : AggregateRoot(id)
{
    public string Code { get; private set; } = string.Empty;
    
    public string Symbol { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;
    
    private CurrencyAggregate(Guid id, string code, string symbol, string name, Guid createdBy) : this(id)
    {
        this.Code = code;
        this.Symbol = symbol;
        this.Name = name;
        this.IsActive = true;
        this.CreatedBy = createdBy;
        this.CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(CurrencyCreatedDomainEvent.Create(Id, Code, Symbol, Name, IsActive));
    }

    public static CurrencyAggregate Create(Guid id, string code, string symbol, string name, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.CurrencyCodeIsInvalid);
        DomainGuard.IsNullOrEmpty(symbol, Errors.SymbolIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        return new CurrencyAggregate(id, code, symbol, name, createdBy);
    }

    public void Update(string code, string symbol, string name, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(code, Errors.CurrencyCodeIsInvalid);
        DomainGuard.IsNullOrEmpty(symbol, Errors.SymbolIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.Code = code;
        this.Symbol = symbol;
        this.Name = name;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(CurrencyUpdatedDomainEvent.Create(Id, Code, Symbol, Name, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsActive = false;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = deletedBy;

        AddEvent(CurrencyDeletedDomainEvent.Create(Id, Code, Symbol, Name, IsActive));
    }
}
