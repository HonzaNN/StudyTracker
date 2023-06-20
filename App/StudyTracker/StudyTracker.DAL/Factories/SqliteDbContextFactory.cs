using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<StudyTrackerDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<StudyTrackerDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;


        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public StudyTrackerDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}