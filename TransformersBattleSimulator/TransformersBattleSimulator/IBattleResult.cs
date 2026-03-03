namespace TransformersBattleSimulator;

public interface IBattleResult : IEntity
{
    string? Winner { get; }
    List<string> Participants { get; }
}
