using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TransformersBattleSimulator;

public class BattleSimulatorDbContextFactory : IDesignTimeDbContextFactory<BattleSimulatorDbContext>
{
    public BattleSimulatorDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("BattleSimulatorDb")
            ?? "Data Source=transformers.db";

        var options = new DbContextOptionsBuilder<BattleSimulatorDbContext>()
            .UseSqlite(connectionString)
            .Options;

        return new BattleSimulatorDbContext(options);
    }
}
