namespace TransformersBattleSimulator;

public interface IRepository
{
    void store<T>(T t);
    void update<T>(T t);
    void retrieve<T>(T t);
    void delete<T>(T t);
}