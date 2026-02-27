namespace TransformersBattleSimulator;

public abstract class TransformerBase(string name, Faction faction) : ITransformer
{
    public string Name { get; set; } = name;

    public int NumberOfWins { get; private set; }
    public int NumberOfLosses { get; private set; }
    public int NumberOfDraws { get; private set; }
    
    public Faction Faction { get; } = faction;

    public void RecordWin() => NumberOfWins++;
    public void RecordLoss() => NumberOfLosses++;
    public void RecordDraw() => NumberOfDraws++;
}