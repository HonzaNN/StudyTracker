using Microsoft.EntityFrameworkCore.Design;

namespace StudyTracker.DAL.Factories;

public class TimeDbContextFactory : IDesignTimeDbContextFactory<StudyTrackerDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory;
    private const string ConnectionString = $"Data Source=StudyTracker;Cache=Shared";

    public TimeDbContextFactory()
    {
        _dbContextSqLiteFactory = new DbContextSqLiteFactory(ConnectionString);
    }

    public StudyTrackerDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}