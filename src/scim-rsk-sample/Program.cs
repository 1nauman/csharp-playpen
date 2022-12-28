using Rsk.AspNetCore.Scim.Configuration;
using Rsk.AspNetCore.Scim.Constants;
using Rsk.AspNetCore.Scim.Models;
using scim_rsk_sample.Models;
using scim_rsk_sample.Stores;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

var services = builder.Services;

var config = builder.Configuration;

var licensee = config.GetValue<string>("RSK.License:Licensee");
var licenseKey = config.GetValue<string>("RSK.License:Key");

services.AddScimServiceProvider("/scim", new ScimLicensingOptions(licensee, licenseKey))
    .AddResource<User, ScimUserStore>("urn:ietf:params:scim:schemas:core:2.0:User", "Users")
    .AddFilterPropertyExpressionCompiler()
    .MapScimAttributes(ScimSchemas.User, mapper =>
    {
        mapper.Map<QapitaUser>("userName", u => u.Email)
            .Map<QapitaUser>("id", u => u.Id)
            .Map<QapitaUser>("name.familyName", u => u.LastName)
            .Map<QapitaUser>("name.givenName", u => u.FirstName)
            .Map<QapitaUser>("active", u => u.Active);
    });

// Build and configure pipeline
var app = builder.Build();

app.UseScim();
app.MapGet("/", () => Results.Ok(new { message = "OK" }));

app.Run();