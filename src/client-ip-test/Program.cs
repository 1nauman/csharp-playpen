using client_ip_test;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (HttpContext httpContext) =>
{
    var (ip, src) = httpContext.GetClientIp();
    return Results.Json(new { ip, src });
});

app.Run();