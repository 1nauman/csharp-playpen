namespace DDD.Sample.Seedwork;

public abstract record DomainEvent(Guid Id, Guid TenantId) 
{
    public DateTime OccuredOn = DateTime.UtcNow;
    
    protected DomainEvent(Guid tenantId) : this(Guid.NewGuid(), tenantId)
    {
    }
}