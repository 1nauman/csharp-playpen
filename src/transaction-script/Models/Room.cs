namespace transaction_script.Models;

public class Room
{
    public int Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public bool Available { get; set; } = true;
}