namespace scim_rsk_sample.Models;

public class QapitaUser
{
    public QapitaUser()
    {
        Id = Guid.NewGuid().ToString("D");
    }

    public QapitaUser(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
    
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public bool Active { get; set; }
}