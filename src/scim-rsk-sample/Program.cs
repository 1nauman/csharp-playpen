using Rsk.AspNetCore.Scim.Configuration;
using Rsk.AspNetCore.Scim.Models;
using scim_rsk_sample.Stores;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var config = builder.Configuration;

var licensee = config.GetValue<string>("RSK.License:Licensee");
var licenseKey = config.GetValue<string>("RSK.License:Key");

services.AddScimServiceProvider("/scim", new ScimLicensingOptions(licensee, licenseKey))
    .AddResource<User, ScimUserStore>("urn:ietf:params:scim:schemas:core:2.0:User", "users");
    //.AddScimDefaultResourcesForInMemoryStore();

// Build and configure pipeline
var app = builder.Build();

app.UseScim();
app.MapGet("/", () => "Hello World!");

app.Run();