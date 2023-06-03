using DDD.Sample.Application.Persistence;
using DDD.Sample.Domain.Issuers;
using DDD.Sample.EventSourcing;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Sample.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class IssuersController : ControllerBase
{
    private readonly IIssuerRepository _issuerRepository;

    public IssuersController(IIssuerRepository issuerRepository)
    {
        _issuerRepository = issuerRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var issuer = new Issuer(Guid.NewGuid(), "Issuer_1", "issuer__1", "india", new DateTime(2000, 1, 1));

        await _issuerRepository.Save(issuer,
            new CommandMetadata(new CorrelationId(Guid.NewGuid()), new CausationId(Guid.NewGuid())));

        return Ok();
    }

    [HttpGet("issuerId:guid")]
    public async Task<IActionResult> GetById(Guid issuerId)
    {
        var issuer = await _issuerRepository.Get(issuerId);
        return Ok(issuer);
    }

    [HttpPut("issuerId:guid")]
    public async Task<IActionResult> UpdateStatus(Guid issuerId)
    {
        var issuer = await _issuerRepository.Get(issuerId);
        
        issuer.ChangeStatus(issuerId, IssuerStatus.Inactive);
        
        await _issuerRepository.Save(issuer,
            new CommandMetadata(new CorrelationId(Guid.NewGuid()), new CausationId(Guid.NewGuid())));

        return Ok();
    }
}