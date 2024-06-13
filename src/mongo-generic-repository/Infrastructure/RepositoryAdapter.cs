using mongo_generic_repository.Seedwork;

namespace mongo_generic_repository.Infrastructure;

public class RepositoryAdapter<TEntity, TStoreEntity, TId> : IRepository<TEntity, TId>
    where TEntity : IEntity<TId>
    where TStoreEntity : IEntity<TId>
{
    private readonly IRepository<TStoreEntity, TId> _storeRepository;
    private readonly IEntityMapper<TEntity, TStoreEntity, TId> _mapper;

    public RepositoryAdapter(
        IRepository<TStoreEntity, TId> storeRepository,
        IEntityMapper<TEntity, TStoreEntity, TId> mapper
    )
    {
        ArgumentNullException.ThrowIfNull(storeRepository);
        ArgumentNullException.ThrowIfNull(mapper);

        _storeRepository = storeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var storeEntities = await _storeRepository.GetAllAsync();
        return await Task.WhenAll(storeEntities.Select(async entity => await _mapper.ToEntity(entity)));
    }

    public async Task<TEntity> GetByIdAsync(TId id)
    {
        var storeEntity = await _storeRepository.GetByIdAsync(id);
        return await _mapper.ToEntity(storeEntity);
    }

    public async Task CreateAsync(TEntity entity)
    {
        var storeEntity = await _mapper.ToStore(entity);
        await _storeRepository.CreateAsync(storeEntity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var storeEntity = await _mapper.ToStore(entity);
        await _storeRepository.UpdateAsync(storeEntity);
    }

    public Task DeleteAsync(TId id) => _storeRepository.DeleteAsync(id);
}