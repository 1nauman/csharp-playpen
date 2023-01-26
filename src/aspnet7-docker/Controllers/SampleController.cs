using System.Net;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace aspnet7_docker.Controllers;

[Route("[controller]")]
public class SampleController : Controller
{
    private readonly IDataProtector _dataProtector;

    public SampleController(IDataProtectionProvider dataProtectionProvider)
    {
        _dataProtector = dataProtectionProvider.CreateProtector("Sample");
    }

    // GET
    [HttpGet]
    public IActionResult Index()
    {
        Response.Cookies.Append("test", _dataProtector.Protect("test"), new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMinutes(5)
        });
        return View();
    }

    [HttpGet("cookie")]
    public IActionResult Cookie()
    {
        Request.Cookies.TryGetValue("test", out var encryptedValue);

        if (string.IsNullOrWhiteSpace(encryptedValue))
        {
            return Ok("No cookie found");
        }

        var plainText = _dataProtector.Unprotect(encryptedValue);

        return Ok(plainText);
    }
}