using Rsk.AspNetCore.Scim.Models;
using Rsk.AspNetCore.Scim.Stores;

namespace scim_rsk_sample.Stores;

public class ScimUserStore : IScimStore<User>
{
    private static readonly List<User> _users = new()
    {
        new User
        {
            Id = Guid.NewGuid().ToString("D"),
            Active = true,
            UserName = "numan@qapita.com"
        }
    };

    public Task<IEnumerable<string>> Exists(IEnumerable<string> ids)
    {
        return Task.FromResult(ids);
    }

    public async Task<User> Add(User resource)
    {
        if (_users.Any(o => o.Id.Equals(resource.Id))) return resource;
        
        _users.Add(resource);
        return resource;
    }

    public Task<User> GetById(string id, ResourceAttributeSet attributes)
    {
        throw new NotImplementedException();
    }

    public async Task<ScimPageResults<User>> GetAll(IResourceQuery query)
    {
        return new ScimPageResults<User>(_users, _users.Count);
    }

    public Task<User> Update(User resource)
    {
        throw new NotImplementedException();
    }

    public Task PartialUpdate(string resourceId, IEnumerable<PatchCommand> updates)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string id)
    {
        throw new NotImplementedException();
    }
}