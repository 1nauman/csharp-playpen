using DDD.Sample.Domain.Issuers;
using DDD.Sample.EventSourcing;

namespace DDD.Sample.Application.Persistence;

public interface IIssuerRepository
{
    Task<Issuer> Get(Guid issuerId);

    Task Save(Issuer issuer, CommandMetadata commandMetadata);
}