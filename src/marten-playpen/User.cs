namespace marten_playpen;

public class User
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
}