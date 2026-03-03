namespace TransformersBattleSimulator;

public class SimpleBattleSimulator(IRepository repository) : IBattleSimulator
{
    public IRepository Repository { get; } = repository;

    public async Task<IBattleResult> BattleAsync(ITransformer a, ITransformer b)
    {
        var winner = Random.Shared.Next(2) == 0 ? a : b;
        winner.RecordWin();
        await Repository.UpdateAsync(RequireEntity(winner));

        var participants = new List<ITransformer> { a, b };
        foreach (var participant in participants)
        {
            if (participant != winner)
            {
                participant.RecordLoss();
                await Repository.UpdateAsync(RequireEntity(participant));
            }
        }

        var result = new SimpleBattleResult(winner.Name, participants.Select(p => p.Name).ToList());
        await Repository.AddAsync(result);
        return result;
    }

    private static TransformerBase RequireEntity(ITransformer transformer)
    {
        if (transformer is TransformerBase entity)
        {
            return entity;
        }

        throw new InvalidOperationException(
            "ITransformer must be backed by TransformerBase for EF repository operations.");
    }
}
