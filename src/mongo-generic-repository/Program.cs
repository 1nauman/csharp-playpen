// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using mongo_generic_repository.Application.Models;
using mongo_generic_repository.Infrastructure;
using mongo_generic_repository.Infrastructure.Persistence.Models;
using mongo_generic_repository.Seedwork;
using MongoDB.Bson.Serialization.Conventions;

ConventionRegistry.Register("CamelCaseConvention",
    new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

var services = new ServiceCollection();

services.AddSingleton<IEncryptionService, EncryptionService>();

//Register your repository
services.AddTransient<IRepository<EmployeePayrollRecordStoreEntity, long>>(
    sp => new MongoRepository<EmployeePayrollRecordStoreEntity, long>(
        "mongodb://localhost:27017",
        "payroll",
        "employee-payroll-records"
    )
);

services
    .AddTransient<IEntityMapper<EmployeePayrollRecord, EmployeePayrollRecordStoreEntity, long>,
        EmployeePayrollRecordMapper>();
services
    .AddTransient<IRepository<EmployeePayrollRecord, long>,
        RepositoryAdapter<EmployeePayrollRecord, EmployeePayrollRecordStoreEntity, long>>();

var provider = services.BuildServiceProvider();

// Actual storage
var payrollRecord = new EmployeePayrollRecord
{
    Id = 12345, // If you're managing IDs yourself
    EmployeeId = 98765,
    BasicSalary = 50000m
};

var adapter = provider.GetRequiredService<IRepository<EmployeePayrollRecord, long>>();

await adapter.CreateAsync(payrollRecord);

var records = await adapter.GetAllAsync();

foreach (var record in records)
{
    Console.WriteLine($"Id: {record.Id}, Employee ID: {record.EmployeeId}, Basic Salary: {record.BasicSalary}");
}