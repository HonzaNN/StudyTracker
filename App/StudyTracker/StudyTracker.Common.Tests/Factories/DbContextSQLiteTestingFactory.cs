using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL;

namespace StudyTracker.Common.Tests.Factories;

public class DbContextSqLiteTestingFactory : IDbContextFactory<StudyTrackerDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public StudyTrackerDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<StudyTrackerDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");


        return new StudyTrackerTestingDbContext(builder.Options, _seedTestingData);
    }
}