// See https://aka.ms/new-console-template for more information

using Akka.Actor;

var system = ActorSystem.Create("ESOPSystem");
var equityAwardPoolActor = system.ActorOf(Props.Create(() => new EquityAwardPoolActor(1000)), "equityAwardPoolActor");
var humanResourcesActor = system.ActorOf(Props.Create(() => new HumanResourcesActor(equityAwardPoolActor)), "humanResourcesActor");

// Simulate awarding options to multiple employees in parallel
var tasks = new List<Task>
{
    Task.Run(() => humanResourcesActor.Tell(new AwardOption(1, 100))),
    Task.Run(() => humanResourcesActor.Tell(new AwardOption(2, 200))),
    Task.Run(() => humanResourcesActor.Tell(new AwardOption(3, 300))),
    Task.Run(() => humanResourcesActor.Tell(new AwardOption(4, 400))),
    Task.Run(() => humanResourcesActor.Tell(new AwardOption(5, 500)))
};

await Task.WhenAll(tasks);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();