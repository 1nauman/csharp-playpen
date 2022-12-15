using System.Text.Json;
using EventStore.Client;

// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

var client = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113"));

// var evt = new TestEvent { Data = "Some string info to persist" };
//
// var evtData = new EventData(Uuid.NewUuid(), nameof(TestEvent), JsonSerializer.SerializeToUtf8Bytes(evt));
//
// await client.AppendToStreamAsync(nameof(TestEvent), StreamState.Any, new[] { evtData });
//
// Console.WriteLine("Written event");

var result = client.ReadStreamAsync(Direction.Forwards, nameof(TestEvent), StreamPosition.Start);

var results = await result.ToListAsync();

foreach (var item in results)
{
    var data = JsonSerializer.Deserialize<TestEvent>(item.Event.Data.Span);
    Console.WriteLine($"{data.Id}---{data.Data}");
}

//Console.ReadKey();

public class TestEvent
{
    public string Id { get; set; } = Guid.NewGuid().ToString("D");
    public string? Data { get; set; }
}