// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using Marten;
using marten_playpen;
using Weasel.Core;

var store = DocumentStore.For("host=localhost;database=marten_test;username=postgres;password=root");

store.Options.AutoCreateSchemaObjects = AutoCreate.All;

store.Options.CreateDatabasesForTenants(c =>
{
    c.ForTenant()
        .CheckAgainstPgDatabase()
        .WithOwner("postgres")
        .WithEncoding("UTF-8")
        .ConnectionLimit(-1);
});

await using var writeSession = store.LightweightSession();
{
    var user = new User
    {
        Id = Guid.NewGuid(),
        FirstName = "Numan",
        LastName = "Mohammed",
        Email = "numan@qapita.com",
        Username = "numanr"
    };

    var role = new Role
    {
        Name = "Test"
    };
    
    writeSession.Store(user);
    writeSession.Store(role);
    writeSession.SaveChanges();
    Console.WriteLine("User saved");
}

using var readSession = store.OpenSession();
var users = readSession.Query<User>();
foreach (var user in users)
{
    Console.WriteLine(user);
}