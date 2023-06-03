using DDD.Sample.Domain.Issuers;
using DDD.Sample.EventSourcing;
using Newtonsoft.Json.Linq;

namespace DDD.Sample.Domain.Events;

public static class EventMappings
{
    private const string Prefix = "issuer";

    public static void MapEventTypes()
    {
        TypeMapper.Map<IssuerCreated>($"{Prefix}-created",
            e => new IssuerCreated(Guid.Parse(e["id"]!.ToString()), e["legalName"]!.ToString(),
                e["brandName"]!.ToString(),
                e["countryOfIncorporation"]!.ToString(), Convert.ToDateTime(e["dateOfIncorporation"])), o => new JObject
            {
                ["id"] = o.Id,
                ["legalName"] = o.LegalName,
                ["brandName"] = o.BrandName,
                ["countryOfIncorporation"] = o.CountryOfIncorporation,
                ["dateOfIncorporation"] = o.DateOfIncorporation
            });

        TypeMapper.Map<IssuerStatusChanged>($"{Prefix}-status-changed",
            e => new IssuerStatusChanged(Guid.Parse(e["id"]!.ToString()),
                Enum.Parse<IssuerStatus>(e["status"]!.ToString())), o => new JObject
            {
                ["id"] = o.Id,
                ["status"] = o.Status.ToString()
            });
    }
}