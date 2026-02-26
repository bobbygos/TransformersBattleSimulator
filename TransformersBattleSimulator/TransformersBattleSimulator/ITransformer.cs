namespace TransformersBattleSimulator;

public interface ITransformer
{
    string Name { get; set; }
    int NumberOfWins { get; }
    int NumberOfLosses { get; }
    string Faction {get; set;}    
}
