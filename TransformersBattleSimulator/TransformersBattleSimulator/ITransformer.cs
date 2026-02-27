namespace TransformersBattleSimulator;

public interface ITransformer
{
    string Name { get; set; }
    int NumberOfWins { get; }
    int NumberOfLosses { get; }
    int NumberOfDraws { get; }
    Faction Faction {get;}
    void RecordWin();
    void RecordLoss();
    void RecordDraw();
}
