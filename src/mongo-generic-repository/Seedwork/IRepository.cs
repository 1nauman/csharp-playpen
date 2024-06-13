namespace mongo_generic_repository.Seedwork;

public interface IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(TId id);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TId id);
}