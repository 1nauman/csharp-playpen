// See https://aka.ms/new-console-template for more information

using Amazon;
using Amazon.SimpleEmailV2;
using aws_ses_emailer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddSingleton<IAmazonSimpleEmailServiceV2>(new AmazonSimpleEmailServiceV2Client(RegionEndpoint.APSouth1));
services.AddTransient<IEmailService, AWSEmailService>();
services.AddLogging(configure => configure.AddConsole());

var serviceProvider = services.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
await emailService.SendAsync("noreply@qapita.com",
    new[]
    {
        "HRMS Dev or Local alerts - Ops Notifications <e7819f77.qapita.com@apac.teams.ms>",
        "numan@qapita.com"
    },
    "Test Email", "This is a test email",
    new List<EmailAttachment>
    {
        new("test.txt", "text/plain", new MemoryStream("This is a test attachment"u8.ToArray()))
    });