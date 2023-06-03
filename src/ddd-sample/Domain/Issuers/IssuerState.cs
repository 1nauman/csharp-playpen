using DDD.Sample.Seedwork;

namespace DDD.Sample.Domain.Issuers;

public class IssuerState : AggregateState
{
    public string LegalName { get; set; }
    
    public string BrandName { get; set; }
    
    public string CountryOfIncorporation { get; set; }
    
    public DateTime DateOfIncorporation { get; set; }
    
    public IssuerStatus Status { get; set; }
    
    public int Version { get; set; }
}

public enum IssuerStatus
{
    Active,
    Inactive
}