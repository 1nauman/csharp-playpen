using Rsk.AspNetCore.Scim.Constants;
using Rsk.AspNetCore.Scim.Filters;
using Rsk.AspNetCore.Scim.Models;
using Rsk.AspNetCore.Scim.Stores;
using scim_rsk_sample.Models;

namespace scim_rsk_sample.Stores;

public class ScimUserStore : IScimStore<User>
{
    private readonly ILogger<ScimUserStore> _logger;
    private readonly IScimQueryBuilderFactory queryBuilderFactory;

    private static readonly List<QapitaUser> _users = new()
    {
        new QapitaUser("80067ac8-b345-4646-8ee1-4d89383bc7ac")
        {
            Active = true,
            Email = "numan@qapita.com",
            FirstName = "Numan",
            LastName = "Mohammed",
        }
    };

    public ScimUserStore(ILogger<ScimUserStore> logger, IScimQueryBuilderFactory queryBuilderFactory)
    {
        _logger = logger;
        this.queryBuilderFactory = queryBuilderFactory;
    }

    public Task<IEnumerable<string>> Exists(IEnumerable<string> ids)
    {
        _logger.LogInformation("Exists {@Ids}", ids);
        return Task.FromResult(ids);
    }

    public async Task<User> Add(User resource)
    {
        _logger.LogInformation("Add {@User}", resource);

        if (_users.Any(o => o.Id.Equals(resource.Id))) return resource;

        var user = MapScimUserToAppUser(resource, new QapitaUser());
        
        _users.Add(user);
        return MapAppUserToScimUser(user);
    }

    public Task<User> GetById(string id, ResourceAttributeSet attributes)
    {
        throw new NotImplementedException();
    }

    public async Task<ScimPageResults<User>> GetAll(IResourceQuery query)
    {
        _logger.LogInformation("GetAll {@Query}", query);

        try
        {
            var dbQuery = queryBuilderFactory.CreateQueryBuilder(_users)
                .Filter(query.Filter)
                .Build();

            var pageQuery = queryBuilderFactory.CreateQueryBuilder(dbQuery)
                .Page(query.StartIndex, query.Count)
                .Sort(query.Sort.By, query.Sort.Direction)
                .Build();

            var totalCount = dbQuery.Count();

            var matchingUser = pageQuery.AsEnumerable().Select(MapAppUserToScimUser).ToList();
            
            return new ScimPageResults<User>(matchingUser, totalCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting filtered users");
            throw;
        }
    }

    public Task<User> Update(User resource)
    {
        _logger.LogInformation("Update {@Resource}", resource);
        throw new NotImplementedException();
    }

    public Task PartialUpdate(string resourceId, IEnumerable<PatchCommand> updates)
    {
        _logger.LogInformation("PartialUpdate {ResourceId}, {@PatchCommands}", resourceId, updates);
        throw new NotImplementedException();
    }

    public Task Delete(string id)
    {
        _logger.LogInformation("Delete {Id}", id);
        throw new NotImplementedException();
    }
    
    private static User MapAppUserToScimUser(QapitaUser user)
    {
        if (user == null)
        {
            return null;
        }

        return new User()
        {
            Id = user.Id,
            UserName = user.Email,
            Active = user.Active,
            Emails = new Email[]
            {
                new() { Display = $"{user.FirstName} {user.LastName}", Primary = true, Value = user.Email }
            },
            Name = new Name
            {
                Formatted = $"{user.FirstName} {user.LastName}",
                FamilyName = user.LastName,
                GivenName = user.FirstName,
            }
        };
    }
    
    private static QapitaUser MapScimUserToAppUser(User resource, QapitaUser user)
    {
        var primaryEmail = resource
            .Emails?
            .SingleOrDefault(e => e.Primary == true)
            ?.Value;

        if (primaryEmail == null)
        {
            primaryEmail = resource.UserName;
        }

        user.Email = primaryEmail ?? user.Email;
        user.FirstName = resource.Name?.GivenName ?? user.FirstName;
        user.LastName = resource.Name?.FamilyName ?? user.LastName;

        user.Active = resource?.Active  ?? user.Active ;
        return user;
    }
}