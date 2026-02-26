namespace TransformersBattleSimulator;

public interface IStorable
{
    // Figure out what this will do exactly. Thought is that using multiple inheritance, the object can tell repository
    // how to handle it. Ie, SimpleTransformer would have a method that will tell repository to store in transformer
    // table (?????)
}