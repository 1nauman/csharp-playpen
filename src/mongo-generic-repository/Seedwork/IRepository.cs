namespace mongo_generic_repository.Seedwork;

/// <summary>
/// Defines a generic repository interface for handling CRUD operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public interface IRepository<TEntity, in TId> where TEntity : IEntity<TId>
{
    /// <summary>
    /// Asynchronously gets all entities of type TEntity.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IEnumerable of TEntity.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Asynchronously gets the entity of type TEntity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TEntity object.</returns>
    Task<TEntity> GetByIdAsync(TId id);

    /// <summary>
    /// Asynchronously creates a new entity of type TEntity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously updates an existing entity of type TEntity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously deletes an entity of type TEntity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(TId id);
}