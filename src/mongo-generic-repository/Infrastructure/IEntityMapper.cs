using mongo_generic_repository.Seedwork;

namespace mongo_generic_repository.Infrastructure;

public interface IEntityMapper<TEntity, TStoreEntity, TId> 
    where TEntity : IEntity<TId>
    where TStoreEntity : IEntity<TId>
{
    Task<TStoreEntity> ToStore(TEntity entity);
    Task<TEntity> ToEntity(TStoreEntity storeEntity);
}