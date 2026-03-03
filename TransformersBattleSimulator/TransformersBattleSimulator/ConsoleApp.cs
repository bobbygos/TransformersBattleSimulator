using Microsoft.EntityFrameworkCore;

namespace TransformersBattleSimulator;

public class ConsoleApp(
    IRepository repository,
    IBattleSimulator battleSimulator,
    BattleSimulatorDbContext dbContext)
{
    public async Task RunAsync()
    {
        var running = true;
        while (running)
        {
            PrintMenu();
            var input = Console.ReadLine()?.Trim();

            try
            {
                switch (input)
                {
                    case "1":
                        await AddTransformerAsync();
                        break;
                    case "2":
                        await RemoveTransformerAsync();
                        break;
                    case "3":
                        await ListTransformersAsync();
                        break;
                    case "4":
                        await RunBattleAsync();
                        break;
                    case "5":
                        await ListBattleResultsAsync();
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("Transformers Battle Simulator");
        Console.WriteLine("1. Add transformer");
        Console.WriteLine("2. Remove transformer");
        Console.WriteLine("3. List transformers");
        Console.WriteLine("4. Run battle");
        Console.WriteLine("5. Show battle results");
        Console.WriteLine("6. Exit");
        Console.Write("Choose an option: ");
    }

    private async Task AddTransformerAsync()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name is required.");
            return;
        }

        //TODO: (Low prio) Consider reworking to make it so that this is auto gen'ed based on enum
        Console.Write("Faction (A=Autobots, D=Decepticons): ");
        var factionInput = Console.ReadLine()?.Trim().ToUpperInvariant();

        TransformerBase? transformer = factionInput switch
        {
            "A" => new AutoBotTransformer(name),
            "D" => new DecepticonTransformer(name),
            _ => null
        };

        if (transformer is null)
        {
            Console.WriteLine("Faction must be A or D.");
            return;
        }

        await repository.AddAsync<TransformerBase>(transformer);
        Console.WriteLine($"Added {transformer.Name} ({transformer.Faction})");
    }
    
    private async Task RemoveTransformerAsync()
    {
        var transformers = await repository.ListAsync<TransformerBase>();

        Console.WriteLine("Available transformers:");
        for (var i = 0; i < transformers.Count; i++)
        {
            var transformer = transformers[i];
            Console.WriteLine($"{i + 1}. {transformer.Name} ({transformer.Faction})");
        }
        var indexRemove = ReadSelection("Select transformer number to remove: ", transformers.Count);

        await repository.DeleteAsync<TransformerBase>(transformers[indexRemove]);
    }


    private async Task ListTransformersAsync()
    {
        var transformers = await repository.ListAsync<TransformerBase>();
        if (transformers.Count == 0)
        {
            Console.WriteLine("No transformers found.");
            return;
        }

        Console.WriteLine("Transformer ID | Transformer Name | Transformer Faction | Wins Losses Draws");

        foreach (var transformer in transformers.OrderBy(t => t.Name))
        {
            Console.WriteLine(
                $"{transformer.Id} | {transformer.Name} | {transformer.Faction} | W:{transformer.NumberOfWins} L:{transformer.NumberOfLosses} D:{transformer.NumberOfDraws}");
        }
    }

    private async Task RunBattleAsync()
    {
        var transformers = await repository.ListAsync<TransformerBase>();
        if (transformers.Count < 2)
        {
            Console.WriteLine("At least two transformers are required.");
            return;
        }

        Console.WriteLine("Available transformers:");
        for (var i = 0; i < transformers.Count; i++)
        {
            var transformer = transformers[i];
            Console.WriteLine($"{i + 1}. {transformer.Name} ({transformer.Faction})");
        }

        var firstIndex = ReadSelection("Select first transformer number: ", transformers.Count);
        var secondIndex = ReadSelection("Select second transformer number: ", transformers.Count);

        if (firstIndex == secondIndex)
        {
            Console.WriteLine("Choose two different transformers.");
            return;
        }

        var a = transformers[firstIndex];
        var b = transformers[secondIndex];
        var result = await battleSimulator.BattleAsync(a, b);

        Console.WriteLine($"Winner: {result.Winner?.Name ?? "Draw"}");
        var names = string.Join(", ", result.Participants.Select(p=> p.Name));
        Console.WriteLine($"Participants: {names}");
    }

    private async Task ListBattleResultsAsync()
    {
        var results = await dbContext.BattleResults
            .Include(r => r.WinnerEntity)
            .Include(r => r.ParticipantEntities)
            .OrderByDescending(r => r.Id)
            .ToListAsync();

        if (results.Count == 0)
        {
            Console.WriteLine("No battle results found.");
            return;
        }

        foreach (var result in results)
        {
            var participants = string.Join(", ", result.ParticipantEntities.Select(p => p.Name));
            Console.WriteLine($"{result.Id} | Winner: {result.WinnerEntity?.Name ?? "None"} | Participants: {participants}");
        }
    }

    private static int ReadSelection(string prompt, int max)
    {
        Console.Write(prompt);
        var raw = Console.ReadLine();
        if (!int.TryParse(raw, out var selection) || selection < 1 || selection > max)
        {
            throw new InvalidOperationException("Invalid selection.");
        }

        return selection - 1;
    }
}
