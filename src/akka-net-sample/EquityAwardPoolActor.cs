using Akka.Actor;

public class EquityAwardPoolActor : ReceiveActor
{
    private int _totalOptions;

    public EquityAwardPoolActor(int initialOptions)
    {
        _totalOptions = initialOptions;

        ReceiveAsync<AwardOption>( async award =>
        {
            //await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(5)));
            
            if (_totalOptions >= award.Options)
            {
                _totalOptions -= award.Options;
                Console.WriteLine(
                    $"Granted {award.Options} options to Employee {award.EmployeeId}. Remaining options: {_totalOptions}");
            }
            else
            {
                Console.WriteLine(
                    $"Not enough options to grant {award.Options} to Employee {award.EmployeeId}. Remaining options: {_totalOptions}");
            }
        });
    }
}