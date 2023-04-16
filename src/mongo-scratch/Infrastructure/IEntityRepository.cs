using System.Linq.Expressions;

namespace mongo_scratch.Infrastructure;

public interface IEntityRepository
{
}

public interface IEntityRepository<TEntity, TId> : IEntityRepository
    where TEntity : IEntity<TId>
    where TId : IComparable
{
    Task<TEntity> Create(Func<Task<TEntity>> entityFactory, string idempotencyKey = null,
        string operationName = null);

    Task Update(
        TId id,
        Func<TEntity, Task> updateAction,
        long? version = null, string idempotencyKey = null,
        string operationName = null);

    Task<IReadOnlyCollection<T>> GetAll<T>(Expression<Func<TEntity, bool>>? filter,
        Expression<Func<TEntity, T>> projection,
        IEnumerable<OrderByClause<TEntity>>? orderByClauses,
        int? startIndex = null, int? limit = null);

    Task<bool> Exists(TId id);

    Task<bool> Exists(Expression<Func<TEntity, bool>> filter);

    Task<TEntity> GetById(TId id);

    Task Delete(TId id);
}