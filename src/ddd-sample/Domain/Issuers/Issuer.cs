using DDD.Sample.Seedwork;

namespace DDD.Sample.Domain.Issuers;

public class Issuer : AggregateRoot<IssuerState>
{
    public string LegalName => State.LegalName;

    public string BrandName => State.BrandName;

    public string CountryOfIncorporation => State.CountryOfIncorporation;

    public DateTime? DateOfIncorporation => State.DateOfIncorporation;


    public Issuer()
    {
        Register<IssuerCreated>(When);
        Register<IssuerStatusChanged>(When);
    }
    
    public Issuer(Guid id, string legalName, string brandName, string countryOfIncorporation,
        DateTime dateOfIncorporation) : this()
    {
        // TODO: Validations
        Raise(new IssuerCreated(id, legalName, brandName, countryOfIncorporation, dateOfIncorporation));
    }
    
    protected Issuer(IssuerState state) : base(state)
    {
    }

    public void ChangeStatus(Guid id, IssuerStatus status)
    {
        // TODO: Validate
        
        Raise(new IssuerStatusChanged(id, status));
    }

    private void When(IssuerCreated evt)
    {
        State.Id = evt.Id;
        State.BrandName = evt.BrandName;
        State.LegalName = evt.LegalName;
        State.CountryOfIncorporation = evt.CountryOfIncorporation;
        State.DateOfIncorporation = evt.DateOfIncorporation;
    }
    
    private void When(IssuerStatusChanged evt)
    {
        State.Status = evt.Status;
    }
}

public record IssuerCreated(Guid Id, string LegalName, string BrandName,
    string CountryOfIncorporation, DateTime DateOfIncorporation) : DomainEvent(Id, Id);

public record IssuerStatusChanged(Guid Id, IssuerStatus Status) : DomainEvent(Id, Id);