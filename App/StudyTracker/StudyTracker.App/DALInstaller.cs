using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyTracker.App.Options;
using StudyTracker.DAL;
using StudyTracker.DAL.Factories;
using StudyTracker.DAL.Mappers;

namespace StudyTracker.App;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
    {
        DALOptions dalOptions = new();
        configuration.GetSection("StudyTracker:DAL").Bind(dalOptions);


        services.AddSingleton(dalOptions);

        if (dalOptions.LocalDb is null && dalOptions.Sqlite is null)
            throw new InvalidOperationException("No persistence provider configured");

        if (dalOptions.LocalDb?.Enabled == false && dalOptions.Sqlite?.Enabled == false)
            throw new InvalidOperationException("No persistence provider enabled");

        if (dalOptions.LocalDb?.Enabled == true && dalOptions.Sqlite?.Enabled == true)
            throw new InvalidOperationException("Both persistence providers enabled");

        if (dalOptions.LocalDb?.Enabled == true)
        {
            services.AddSingleton<IDbContextFactory<StudyTrackerDbContext>>(provider =>
                new DbContextSqLiteFactory(dalOptions.LocalDb.ConnectionString));
            services.AddSingleton<IDbMigrator, NoneDbMigrator>();
        }

        if (dalOptions.Sqlite?.Enabled == true)
        {
            if (dalOptions.Sqlite.DatabaseName is null)
                throw new InvalidOperationException($"{nameof(dalOptions.Sqlite.DatabaseName)} is not set");

            var databaseFilePath = Path.Combine(FileSystem.AppDataDirectory, dalOptions.Sqlite.DatabaseName!);
            services.AddSingleton<IDbContextFactory<StudyTrackerDbContext>>(provider =>
                new DbContextSqLiteFactory(databaseFilePath, dalOptions?.Sqlite?.SeedDemoData ?? false));
            services.AddSingleton<IDbMigrator, SqliteDbMigrator>();
        }

        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<ActivityToUserMapper>();
        services.AddSingleton<ActivityEntityMapper>();
        services.AddSingleton<SubjectEntityMapper>();
        services.AddSingleton<SubjectToUserMapper>();

        return services;
    }
}