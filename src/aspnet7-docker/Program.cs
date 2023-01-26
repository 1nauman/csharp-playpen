using Microsoft.AspNetCore.DataProtection;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

// Add services to the container.
    builder.Services.AddDataProtection(options =>
        {
            options.ApplicationDiscriminator = "aspnet7-docker";
        })
        .SetDefaultKeyLifetime(TimeSpan.FromDays(15)) // This order is important, if this call is made after the Persist to SSM then it has no effect.
        .PersistKeysToAWSSystemsManager("/aspnet-ssm-playpen/data-protection-keys/");
    
    builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}