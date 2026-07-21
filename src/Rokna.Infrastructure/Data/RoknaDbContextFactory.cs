using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using Rokna.Domain.Entities;
using Rokna.Infrastructure.Data;

namespace Rokna.Infrastructure.Data;
public class RoknaDbContextFactory : IDbContextFactory<RoknaDbContext>, IDesignTimeDbContextFactory<RoknaDbContext>
{
    private readonly string _connectionString;

    public RoknaDbContextFactory()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dbPath = Path.Combine(appData, "RoknaCafe", "RoknaCafe.db");
        
        var directory = Path.GetDirectoryName(dbPath);
        if (!Directory.Exists(directory)) 
        {
            Directory.CreateDirectory(directory);
        
        }
        
        
        _connectionString = $"Data Source={dbPath}";
    }
    public RoknaDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<RoknaDbContext>();
        optionsBuilder.UseSqlite(_connectionString);

        return new RoknaDbContext(optionsBuilder.Options);
    }

    public RoknaDbContext CreateDbContext(string[] args)
    {
        return CreateDbContext();
    }

}