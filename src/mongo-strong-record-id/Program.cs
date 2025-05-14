// See https://aka.ms/new-console-template for more information

using MongoDB.Driver;

var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("testdb");
var collection = database.GetCollection<Person>("persons");

var results = await collection.FindAsync(o => o.Id == "df5f3054-61f9-4239-94e0-64abb687d9f1");

await results.ForEachAsync(o =>
{
    Console.WriteLine($"Id: {o.Id}, Name: {o.Name}, Age: {o.Age}");
});

// var person = new Person
// {
//     Id = PersonId.NewId(),
//     Name = "John Doe",
//     Age = 30
// };

//await collection.InsertOneAsync(person);

public class Person
{
    public PersonId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

public readonly record struct PersonId(string Value)
{
    public static PersonId NewId() => new(Guid.NewGuid().ToString());
    public static implicit operator string(PersonId id) => id.Value;
    public static implicit operator PersonId(string id) => new(id);
}