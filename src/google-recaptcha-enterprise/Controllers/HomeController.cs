using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using google_recaptcha_enterprise.Models;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.RecaptchaEnterprise.V1;

namespace google_recaptcha_enterprise.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GoogleReCaptchaEnterpriseConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, GoogleReCaptchaEnterpriseConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(IndexModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await AssessAction(model.ClientToken, "submit");

        return View();
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

    private async Task<bool> AssessAction(string tokenFromClient, string action)
    {
        var request = new CreateAssessmentRequest
        {
            ParentAsProjectName = ProjectName.FromProject(_configuration.ProjectId),
            Assessment = new Assessment
            {
                Event = new Event
                {
                    SiteKey = _configuration.SiteKey,
                    Token = tokenFromClient,
                    ExpectedAction = action
                }
            }
        };

        var client = await RecaptchaEnterpriseServiceClient.CreateAsync();
        var result = await client.CreateAssessmentAsync(request);
        
        _logger.LogInformation("Assessment result: {@Result}", result);
        
        return result.TokenProperties.Valid && result.TokenProperties.Action.Equals(action) &&
               result.RiskAnalysis?.Score >= 0.5;
    }
}