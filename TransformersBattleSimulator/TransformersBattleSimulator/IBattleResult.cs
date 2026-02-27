namespace TransformersBattleSimulator;

public interface IBattleResult
{
    ITransformer? Winner { get; }
    List<ITransformer> Participants { get; }
}