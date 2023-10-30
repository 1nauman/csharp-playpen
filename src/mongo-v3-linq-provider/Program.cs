// See https://aka.ms/new-console-template for more information

using Bogus;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var faker = new Faker<mongo_v3_linq_provider.Person>()
    .RuleFor(o => o.Id, f => f.Random.Guid())
    .RuleFor(o => o.FirstName, f => f.Name.FirstName())
    .RuleFor(o => o.LastName, f => f.Name.LastName())
    .RuleFor(o => o.BirthDate, f => f.Date.Past(100))
    .RuleFor(o => o.SomeNumber, f => f.Random.Int(0, 100) < 10 ? null : f.Random.Int(0, 100))
    .RuleFor(o => o.Email, f => f.Internet.Email());

//faker.Generate(10).ToList().ForEach(Console.WriteLine);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
var settings = new MongoClientSettings
{
    Server = new MongoServerAddress("localhost", 27017),
};

var client = new MongoClient(settings);

var database = client.GetDatabase("test");

var collection = database.GetCollection<mongo_v3_linq_provider.Person>("people");

//await collection.InsertManyAsync(faker.Generate(Random.Shared.Next(10000, 100000)));

Console.WriteLine("All people: born after 1947");

collection.AsQueryable().Where(o => o.SomeNumber.Equals(1)).ToList().ForEach(Console.WriteLine);