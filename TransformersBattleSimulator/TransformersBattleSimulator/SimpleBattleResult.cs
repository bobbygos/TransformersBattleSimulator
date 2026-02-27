namespace TransformersBattleSimulator;

public class SimpleBattleResult(ITransformer winner, List<ITransformer> participants) : IBattleResult
{
    public ITransformer Winner { get; } = winner;
    public List<ITransformer> Participants { get; } = participants;
}