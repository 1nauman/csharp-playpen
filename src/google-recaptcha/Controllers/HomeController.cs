using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using google_recaptcha.Models;

namespace google_recaptcha.Controllers;

public class HomeController : Controller
{
    private readonly IReCaptchaService _reCaptchaService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IReCaptchaService reCaptchaService)
    {
        _logger = logger;
        _reCaptchaService = reCaptchaService;
    }

    public IActionResult Index()
    {
        return View(new TestModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(TestModel model)
    {
        Console.WriteLine($"Client Token: {model.ClientToken}");
        if (!ModelState.IsValid) return View(model);

        var validationResult = await _reCaptchaService.VerifyAsync(model.ClientToken);

        model.Message =
            $"Validation passed: {validationResult.Score}, {validationResult.Action} {validationResult.Hostname} {validationResult.ChallengeTimestamp}";

        if (validationResult.Success)
        {
            model.ClientToken = string.Empty;
            return View(model);
        }

        ModelState.AddModelError("ReCaptcha", "reCaptcha validation failed");
        
        _logger.LogWarning("ReCaptcha validation failed: {@Details}", validationResult);
        
        model.ClientToken = string.Empty;
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}