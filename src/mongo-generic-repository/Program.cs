// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using mongo_generic_repository.Application.Models;
using mongo_generic_repository.Infrastructure;
using mongo_generic_repository.Infrastructure.Persistence.Models;

//var services = new ServiceCollection();

// Register your repository
// services.AddScoped<IRepository<EmployeePayrollRecord, long>>(
//     sp => new MongoRepository<EmployeePayrollRecord, long>(
//         "mongodb://localhost:27017",
//         "payroll",
//         "employee-payroll-records"
//     )
// );
//
// var provider = services.BuildServiceProvider();

// Actual storage
var payrollRecord = new EmployeePayrollRecord
{
    Id = 12345, // If you're managing IDs yourself
    EmployeeId = 98765,
    BasicSalary = 50000m
};

var repo = new MongoRepository<EmployeePayrollRecordStoreEntity, long>(
    "mongodb://localhost:27017",
    "payroll",
    "employee-payroll-records"
);

var mapper = new EmployeePayrollRecordMapper();
var adapter = new RepositoryAdapter<EmployeePayrollRecord, EmployeePayrollRecordStoreEntity, long>(repo, mapper);

await adapter.CreateAsync(payrollRecord);

var records = await adapter.GetAllAsync();

foreach (var record in records)
{
    Console.WriteLine($"Employee ID: {record.EmployeeId}, Basic Salary: {record.BasicSalary}");
}