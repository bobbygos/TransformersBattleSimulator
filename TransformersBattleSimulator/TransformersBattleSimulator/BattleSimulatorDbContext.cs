using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TransformersBattleSimulator;

public class BattleSimulatorDbContext(DbContextOptions<BattleSimulatorDbContext> options) : DbContext(options)
{
    public DbSet<TransformerBase> Transformers => Set<TransformerBase>();
    public DbSet<SimpleBattleResult> BattleResults => Set<SimpleBattleResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransformerBase>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.HasDiscriminator<string>("TransformerType")
                .HasValue<AutoBotTransformer>(nameof(AutoBotTransformer))
                .HasValue<DecepticonTransformer>(nameof(DecepticonTransformer));
        });

        modelBuilder.Entity<SimpleBattleResult>(entity =>
        {
            entity.HasKey(br => br.Id);
            entity.Property(br => br.Winner)
                .HasMaxLength(200);
            
            var converter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => string.IsNullOrWhiteSpace(v)
                    ? new List<string> { }
                    : JsonSerializer.Deserialize<List<string>>(v) ?? new List<string> { });

            var comparer = new ValueComparer<List<string>>(
                (a, b) => (a ?? new List<string> { }).SequenceEqual(b ?? new List<string> { }),
                v => (v).Aggregate(0, (h, s) => HashCode.Combine(h, s.GetHashCode())),
                v => (v).ToList());

            entity.Property(x => x.Participants)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);
        });
    }
}
