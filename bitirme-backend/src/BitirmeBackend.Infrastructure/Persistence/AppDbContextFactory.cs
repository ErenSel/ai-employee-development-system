using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BitirmeBackend.Infrastructure.Persistence;

/// <summary>
/// Design-time factory so `dotnet ef migrations`/`database update` can construct the
/// context without spinning up the web host. The connection string is read from the
/// ConnectionStrings__DefaultConnection environment variable, falling back to the local
/// development default documented in appsettings.json.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=bitirme_db;Username=postgres;Password=1234";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new AppDbContext(options);
    }
}
