// See https://aka.ms/new-console-template for more information

using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var guid = Guid.Parse("0922f99b-1f8d-43f0-baf1-2c34cc4b995b");

// Convert GUID to BSON
BsonBinaryData bsonBinaryData = new BsonBinaryData(guid, GuidRepresentation.Standard);
Console.WriteLine("BSON Binary Data: " + bsonBinaryData);

var base64String = Convert.ToBase64String(bsonBinaryData.Bytes);
Console.WriteLine("Base64 String: " + base64String);

//var base64Bytes = Convert.FromBase64String(base64String);
var base64Bytes = Convert.FromBase64String("EXRbzMeGSkuRag7FHEF3Rw==");
// Convert Base64 back to BSON Binary Data
var bsonBinaryData2 = new BsonBinaryData(base64Bytes, BsonBinarySubType.UuidStandard);

// Convert BSON back to GUID
Guid convertedGuid = bsonBinaryData2.ToGuid(GuidRepresentation.Standard);
Console.WriteLine("Converted GUID: " + convertedGuid);

/*using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mongo_scratch.Infrastructure;
using mongo_scratch.Models;

var host = Host.CreateDefaultBuilder().ConfigureServices((_, services) =>
{
    services.TryAddSingleton<IReportModelDBSettings>(new ReportModelDBSettings
    {
        ConnectionString = "mongodb://localhost:27017",
        DatabaseName = "mongo_scratch"
    });

    services.TryAddSingleton<IReportMongoContext>(sp =>
    {
        var context = new ReportMongoContext(sp.GetRequiredService<IReportModelDBSettings>(),
            sp.GetRequiredService<ILogger<ReportMongoContext>>());

        context.GetGenericCollection<Employee>();

        return context;
    });

    services.Scan(scan =>
    {
        scan.FromAssemblyOf<Employee>()
            .AddClasses(classes => classes.AssignableTo<IEntityRepository>())
            .AsImplementedInterfaces()
            .WithTransientLifetime();
    });
}).Build();

var empRepo = host.Services.GetRequiredService<IEmployeeRepository>();
await empRepo.Create(() =>
{
    var emp = new Employee(new EmployeeId(Guid.NewGuid().ToString("D")))
    {
        DateOfBirth = DateTime.Today.AddYears(-18).Date,
        Department = "IT",
        Name = new PersonName
        {
            FirstName = "Nauman",
            LastName = "Mohammed"
        }
    };

    return Task.FromResult(emp);
});

await host.RunAsync();*/