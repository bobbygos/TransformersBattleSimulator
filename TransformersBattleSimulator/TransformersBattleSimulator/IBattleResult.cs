namespace TransformersBattleSimulator;

public interface IBattleResult : IEntity
{
    ITransformer? Winner { get; }
    List<ITransformer> Participants { get; }
}
