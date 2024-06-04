// See https://aka.ms/new-console-template for more information

using Audit.MongoClient;
using audit.net_mongo_client;
using MongoDB.Driver;

var mongoClientSettings = new MongoClientSettings
{
    Server = new MongoServerAddress("localhost", 27017),
    ClusterConfigurator = clusterBuilder => clusterBuilder.AddAuditSubscriber(ac => ac.IncludeReply())
};

var _client = new MongoClient(mongoClientSettings);

var db = _client.GetDatabase("audit_test");

await db.GetCollection<Person>("persons").InsertOneAsync(new Person
{
    FirstName = "Numan",
    LastName = "Mohammed"
});

