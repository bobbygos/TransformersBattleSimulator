namespace TransformersBattleSimulator;

public interface IBattleSimulator
{
    IBattleResult Battle(ITransformer a, ITransformer b);
}