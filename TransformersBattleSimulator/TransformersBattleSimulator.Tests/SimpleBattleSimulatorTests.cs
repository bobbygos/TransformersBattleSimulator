using TransformersBattleSimulator;
using Xunit;

namespace TransformersBattleSimulator.Tests;

public class SimpleBattleSimulatorTests
{
    [Fact]
    public async Task BattleAsync_ReturnsResultWithTwoParticipantsAndWinnerAmongThem()
    {
        var repository = new FakeRepository();
        var simulator = new SimpleBattleSimulator(repository);
        var a = new AutoBotTransformer("Optimus Prime");
        var b = new DecepticonTransformer("Megatron");

        await repository.AddAsync<TransformerBase>(a);
        await repository.AddAsync<TransformerBase>(b);

        var result = await simulator.BattleAsync(a, b);

        Assert.NotNull(result.Winner);
        Assert.Equal(2, result.Participants.Count);
        Assert.Contains(result.Winner, result.Participants);
    }

    [Fact]
    public async Task BattleAsync_UpdatesExactlyOneWinAndOneLoss()
    {
        var repository = new FakeRepository();
        var simulator = new SimpleBattleSimulator(repository);
        var a = new AutoBotTransformer("Bumblebee");
        var b = new DecepticonTransformer("Starscream");

        await repository.AddAsync<TransformerBase>(a);
        await repository.AddAsync<TransformerBase>(b);

        await simulator.BattleAsync(a, b);

        Assert.Equal(1, a.NumberOfWins + b.NumberOfWins);
        Assert.Equal(1, a.NumberOfLosses + b.NumberOfLosses);
        Assert.Equal(0, a.NumberOfDraws + b.NumberOfDraws);
    }

    [Fact]
    public async Task BattleAsync_UpdatesBothTransformersAndStoresOneBattleResult()
    {
        var repository = new FakeRepository();
        var simulator = new SimpleBattleSimulator(repository);
        var a = new AutoBotTransformer("Ironhide");
        var b = new DecepticonTransformer("Soundwave");

        await repository.AddAsync<TransformerBase>(a);
        await repository.AddAsync<TransformerBase>(b);

        var result = await simulator.BattleAsync(a, b);

        var storedResults = await repository.ListAsync<SimpleBattleResult>();
        Assert.Single(storedResults);
        Assert.Same(result, storedResults.Single());
        Assert.Equal(2, repository.UpdateCalls);
    }

    private sealed class FakeRepository : IRepository
    {
        private readonly Dictionary<Type, List<IEntity>> _store = [];

        public int UpdateCalls { get; private set; }

        public Task<T> AddAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity
        {
            if (!_store.TryGetValue(typeof(T), out var entities))
            {
                entities = [];
                _store[typeof(T)] = entities;
            }

            entities.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<T?> GetByIdAsync<T>(Guid id, CancellationToken ct = default) where T : class, IEntity
        {
            if (!_store.TryGetValue(typeof(T), out var entities))
            {
                return Task.FromResult<T?>(null);
            }

            var found = entities.Cast<T>().FirstOrDefault(entity => entity.Id == id);
            return Task.FromResult(found);
        }

        public Task<List<T>> ListAsync<T>(CancellationToken ct = default) where T : class, IEntity
        {
            if (!_store.TryGetValue(typeof(T), out var entities))
            {
                return Task.FromResult(new List<T>());
            }

            return Task.FromResult(entities.Cast<T>().ToList());
        }

        public Task UpdateAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity
        {
            UpdateCalls++;
            return Task.CompletedTask;
        }

        public Task DeleteAsync<T>(T entity, CancellationToken ct = default) where T : class, IEntity
        {
            if (_store.TryGetValue(typeof(T), out var entities))
            {
                entities.Remove(entity);
            }

            return Task.CompletedTask;
        }
    }
}
