using Akka.Actor;

public class EmployeeActor : ReceiveActor
{
    private readonly IActorRef _equityAwardPoolActor;

    public EmployeeActor(IActorRef equityAwardPoolActor)
    {
        _equityAwardPoolActor = equityAwardPoolActor;

        ReceiveAsync<AwardOption>( async award =>
        {
            //await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(5)));  // Simulate work
            _equityAwardPoolActor.Tell(award);
        });
    }
}