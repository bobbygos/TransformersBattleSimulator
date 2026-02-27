namespace TransformersBattleSimulator;

public interface IBattleSimulator
{
    public IRepository Repository { get; }
    IBattleResult Battle(ITransformer a, ITransformer b);
}