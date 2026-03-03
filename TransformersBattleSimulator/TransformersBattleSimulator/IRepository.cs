namespace TransformersBattleSimulator;

public interface IRepository
{
    Task<T> AddAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity;
    Task<List<T>> ListAsync<T>(CancellationToken ct = default) where T : class, IEntity;
    Task UpdateAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity;
    Task DeleteAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity;
}