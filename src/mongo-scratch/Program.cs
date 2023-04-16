// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
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

await host.RunAsync();