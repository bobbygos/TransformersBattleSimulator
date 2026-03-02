using Microsoft.EntityFrameworkCore;

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
            entity.Ignore(br => br.Winner);
            entity.Ignore(br => br.Participants);

            entity.HasOne(br => br.WinnerEntity)
                .WithMany()
                .HasForeignKey(br => br.WinnerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(br => br.ParticipantEntities)
                .WithMany()
                .UsingEntity(j => j.ToTable("BattleResultParticipants"));
        });
    }
}
