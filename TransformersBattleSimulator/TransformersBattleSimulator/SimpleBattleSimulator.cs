using System;

namespace TransformersBattleSimulator;

public class SimpleBattleSimulator(IRepository repository) : IBattleSimulator
{
    public IRepository Repository { get; } = repository;
    
    public IBattleResult Battle(ITransformer a, ITransformer b)
    {
        var winner = Random.Shared.Next(2) == 0 ? a : b;
        winner.RecordWin();
        Repository.update(winner);
        
        var participants = new List<ITransformer>{a, b};
        foreach (var participant in participants)
        {
            if (participant != winner)
            {
                participant.RecordLoss();
                Repository.update(participant);
            }
        }

        var result = new SimpleBattleResult(winner, participants);
        Repository.store(result);
        return result;
    }
}