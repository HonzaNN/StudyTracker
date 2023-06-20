using Microsoft.EntityFrameworkCore;
using StudyTracker.App.Options;
using StudyTracker.DAL;

namespace StudyTracker.App;

internal interface IDbMigrator
{
    public void Migrate();
    public Task MigrateAsync(CancellationToken cancellationToken);
}

public class NoneDbMigrator : IDbMigrator
{
    public void Migrate()
    {
    }

    public Task MigrateAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class SqliteDbMigrator : IDbMigrator
{
    private readonly IDbContextFactory<StudyTrackerDbContext> _dbContextFactory;
    private readonly SqliteOptions _sqliteOptions;

    public SqliteDbMigrator(IDbContextFactory<StudyTrackerDbContext> dbContextFactory, DALOptions dalOptions)
    {
        _dbContextFactory = dbContextFactory;
        _sqliteOptions = dalOptions.Sqlite ??
                         throw new ArgumentNullException(nameof(dalOptions),
                             $@"{nameof(DALOptions.Sqlite)} are not set");
    }

    public void Migrate()
    {
        MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        if (_sqliteOptions.RecreateDatabaseEachTime) await dbContext.Database.EnsureDeletedAsync(cancellationToken);

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}