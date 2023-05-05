// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using Marten;
using marten_playpen;
using Weasel.Core;

var store = DocumentStore.For("host=localhost;database=marten_test;username=postgres;password=root");

store.Options.Policies.AllDocumentsAreMultiTenanted();

store.Options.AutoCreateSchemaObjects = AutoCreate.All;

store.Options.CreateDatabasesForTenants(c =>
{
    c.ForTenant()
        .CheckAgainstPgDatabase()
        .WithOwner("postgres")
        .WithEncoding("UTF-8")
        .ConnectionLimit(-1);
});

long clientId = 1080L;

// await using var writeSession = store.LightweightSession(clientId.ToString());
// {
//     var user = new User
//     {
//         Id = Guid.NewGuid(),
//         FirstName = "Numan",
//         LastName = $"Mohammed {clientId}",
//         Email = "numan@qapita.com",
//         Username = "numanr",
//         ClientId = clientId
//     };
//
//     var role = new Role
//     {
//         Name = "Test",
//         ClientId = clientId
//     };
//     
//     writeSession.Store(user);
//     writeSession.Store(role);
//     writeSession.SaveChanges();
//     Console.WriteLine("User saved");
// }

using var readSession = store.OpenSession(clientId.ToString());
var users = readSession.Query<User>();
foreach (var user in users)
{
    Console.WriteLine(user);
}