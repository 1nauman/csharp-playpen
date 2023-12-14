// See https://aka.ms/new-console-template for more information

using Marten;
using marten_playpen;
using Marten.Services;
using Marten.Services.Json;
using Newtonsoft.Json;
using Weasel.Core;

var martenSerializer = new JsonNetSerializer();
martenSerializer.Customize(serializer =>
{
    serializer.Converters.Add(new NewtonsoftEnumerationValueSerializer());
    serializer.TypeNameHandling = TypeNameHandling.Auto;
    serializer.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
    serializer.MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead;
    serializer.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
    serializer.ContractResolver = new JsonNetContractResolver();
});

var store = DocumentStore.For(storeOptions =>
{
    storeOptions.Connection("host=localhost;database=marten_test;username=postgres;password=postgres");
    storeOptions.Serializer(martenSerializer);
    storeOptions.AutoCreateSchemaObjects = AutoCreate.All;
    storeOptions.Policies.AllDocumentsAreMultiTenanted();
    storeOptions.Linq.FieldSources.Add(new CustomStringFieldSource());
});

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
// var user = new User
// {
//     Id = Guid.NewGuid(),
//     FirstName = "Numan",
//     LastName = $"Mohammed {clientId}",
//     Email = "someone@test.com",
//     Username = "numanr",
//     ClientId = clientId,
// };
//
// var role = new Role
// {
//     Name = "Test",
//     ClientId = clientId
// };
//
// writeSession.Store(user);
// writeSession.Store(role);
// writeSession.SaveChanges();
// Console.WriteLine("User saved");

using var readSession = store.OpenSession(clientId.ToString());
var users = readSession.Query<User>().Where(o => o.Status == Status.Inactive);
foreach (var u in users)
{
    Console.WriteLine(u);
}