namespace CodeDesignPlus.Net.Microservice.Locations.Domain;

public class CurrencyAggregate(Guid id) : AggregateRootBase(id)
{
    public string Code { get; private set; } = string.Empty;
    public short NumericCode { get; private set; }
    public short DecimalDigits { get; private set; }
    public string Symbol { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public static CurrencyAggregate Create(Guid id, string code, short numericCode, short decimalDigits, string symbol, string name, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.IdIsInvalid);
        DomainGuard.IsNullOrEmpty(code, Errors.CurrencyCodeIsInvalid);
        DomainGuard.IsNullOrEmpty(symbol, Errors.SymbolIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(createdBy, Errors.CreatedByIsInvalid);

        var aggregate = new CurrencyAggregate(id)
        {
            Code = code,
            NumericCode = numericCode,
            DecimalDigits = decimalDigits,
            Symbol = symbol,
            Name = name,
            IsActive = true,
            CreatedBy = createdBy,
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
        };

        aggregate.AddEvent(CurrencyCreatedDomainEvent.Create(id, name, code, numericCode, decimalDigits, symbol, aggregate.IsActive));

        return aggregate;
    }

    public void Update(string code, short numericCode, short decimalDigits, string symbol, string name, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(code, Errors.CurrencyCodeIsInvalid);
        DomainGuard.IsNullOrEmpty(symbol, Errors.SymbolIsInvalid);
        DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.UpdateByIsInvalid);

        this.Code = code;
        this.NumericCode = numericCode;
        this.DecimalDigits = decimalDigits;
        this.Symbol = symbol;
        this.Name = name;
        this.IsActive = isActive;
        this.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        this.UpdatedBy = updatedBy;

        AddEvent(CurrencyUpdatedDomainEvent.Create(Id, Name, Code, NumericCode, DecimalDigits, Symbol, IsActive));
    }

    public void Delete(Guid deletedBy)
    {
        DomainGuard.GuidIsEmpty(deletedBy, Errors.DeleteByIsInvalid);

        this.IsDeleted = true;
        this.IsActive = false;
        this.DeletedAt = SystemClock.Instance.GetCurrentInstant();
        this.DeletedBy = deletedBy;

        AddEvent(CurrencyDeletedDomainEvent.Create(Id, Name, Code, NumericCode, DecimalDigits, Symbol, IsActive));
    }
}
