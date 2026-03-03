namespace TransformersBattleSimulator;

public class SimpleBattleResult : IBattleResult
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string? Winner { get; private set; }
    public List<string> Participants { get; private set; } = [];

    private SimpleBattleResult()
    {
    }

    public SimpleBattleResult(string? winner, List<string> participants)
    {
        ArgumentNullException.ThrowIfNull(participants);

        if (participants.Count == 0)
        {
            throw new ArgumentException("Participants cannot be empty.", nameof(participants));
        }

        Winner = string.IsNullOrWhiteSpace(winner) ? null : winner.Trim();
        Participants = participants;
    }
}
