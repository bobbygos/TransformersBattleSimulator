using Microsoft.EntityFrameworkCore;

namespace TransformersBattleSimulator;

public class DatabaseRepository(BattleSimulatorDbContext dbContext) : IRepository
{
    public async Task<T> AddAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity
    {
        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync(ct);
        return entity;
    }

    public Task<List<T>> ListAsync<T>(CancellationToken ct = default) where T : class, IEntity
    {
        return dbContext.Set<T>().ToListAsync(ct);
    }

    public async Task UpdateAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
