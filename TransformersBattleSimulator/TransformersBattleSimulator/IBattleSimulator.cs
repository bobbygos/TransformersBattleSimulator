namespace TransformersBattleSimulator;

public interface IBattleSimulator
{
    public IRepository Repository { get; }
    Task<IBattleResult> BattleAsync(ITransformer a, ITransformer b);
}
