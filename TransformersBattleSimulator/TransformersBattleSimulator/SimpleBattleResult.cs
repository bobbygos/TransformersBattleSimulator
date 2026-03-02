namespace TransformersBattleSimulator;

public class SimpleBattleResult : IBattleResult
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid? WinnerId { get; private set; }
    public TransformerBase? WinnerEntity { get; private set; }

    public List<TransformerBase> ParticipantEntities { get; private set; } = [];

    public ITransformer? Winner => WinnerEntity;
    public List<ITransformer> Participants => ParticipantEntities.Cast<ITransformer>().ToList();

    private SimpleBattleResult()
    {
    }

    public SimpleBattleResult(ITransformer winner, List<ITransformer> participants)
    {
        if (winner is not TransformerBase winnerEntity)
        {
            throw new ArgumentException("Winner must inherit from TransformerBase.", nameof(winner));
        }

        var participantEntities = new List<TransformerBase>(participants.Count);
        foreach (var participant in participants)
        {
            if (participant is not TransformerBase entity)
            {
                throw new ArgumentException("All participants must inherit from TransformerBase.", nameof(participants));
            }

            participantEntities.Add(entity);
        }

        WinnerEntity = winnerEntity;
        WinnerId = winnerEntity.Id;
        ParticipantEntities = participantEntities;
    }
}
