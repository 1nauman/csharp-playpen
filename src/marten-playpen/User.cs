namespace marten_playpen;

public interface IClientAware
{
    long ClientId { get; }
}

public class User : IClientAware
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }

    public override string ToString()
    {
        return $"Id={Id:D}, FirstName={FirstName}, LastName={LastName}, Email={Email}, Username={Username}";
    }

    public long ClientId { get; set; }
}