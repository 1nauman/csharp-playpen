using Akka.Actor;

public class HumanResourcesActor : ReceiveActor
{
    private readonly IActorRef _equityAwardPoolActor;
    private readonly Dictionary<int, IActorRef> _employeeActors;

    public HumanResourcesActor(IActorRef equityAwardPoolActor)
    {
        _equityAwardPoolActor = equityAwardPoolActor;
        _employeeActors = new Dictionary<int, IActorRef>();

        Receive<AwardOption>(award =>
        {
            if (!_employeeActors.ContainsKey(award.EmployeeId))
            {
                var employeeActor = Context.ActorOf(Props.Create(() => new EmployeeActor(_equityAwardPoolActor)));
                _employeeActors.Add(award.EmployeeId, employeeActor);
            }

            _employeeActors[award.EmployeeId].Tell(award);
        });
    }
}