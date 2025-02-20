using System.Reflection;
using DbUp;

namespace LibraryManagementSystem.Data;

public static class DbInitializer
{
    public static void Initialize(string connectionString)
    {
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .LogToConsole()
            .Build();

        var migrationsResults = upgrader.PerformUpgrade();

        if (!migrationsResults.Successful) {
            throw new InvalidOperationException($"Migrations failed: {migrationsResults}");
        }
    }
}
