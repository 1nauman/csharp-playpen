namespace mongo_generic_repository.Seedwork;

public interface IEntity<out TId>
{
    public TId Id { get; }
}