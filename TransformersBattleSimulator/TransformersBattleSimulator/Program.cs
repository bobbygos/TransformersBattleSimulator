using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TransformersBattleSimulator;

public static class Program
{
    public static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("BattleSimulatorDb")
            ?? throw new InvalidOperationException("Missing connection string: ConnectionStrings:BattleSimulatorDb");

        var services = new ServiceCollection();

        services.AddDbContext<BattleSimulatorDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IRepository, DatabaseRepository>();
        services.AddScoped<IBattleSimulator, SimpleBattleSimulator>();
        services.AddScoped<ConsoleApp>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var db = provider.ServiceProvider.GetRequiredService<BattleSimulatorDbContext>();
        await db.Database.EnsureCreatedAsync();

        var app = provider.ServiceProvider.GetRequiredService<ConsoleApp>();
        await app.RunAsync();
    }
}
