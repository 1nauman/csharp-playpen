namespace marten_playpen;

public class Role : IClientAware
{
    public long Id { get; set; }

    public string Name { get; set; }
    
    public long ClientId { get; set; }
}