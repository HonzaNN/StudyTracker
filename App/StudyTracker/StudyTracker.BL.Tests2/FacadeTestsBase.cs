using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Mappers;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.Common.Tests;
using StudyTracker.Common.Tests.Factories;
using StudyTracker.DAL;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.UnitOfWork;
using Xunit.Abstractions;

namespace StudyTracker.BL.Tests2;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);


        ActivityEntityMapper = new ActivityEntityMapper();
        ActivityToUserMapper = new ActivityToUserMapper();
        SubjectEntityMapper = new SubjectEntityMapper();
        SubjectToUserMapper = new SubjectToUserMapper();
        UserEntityMapper = new UserEntityMapper();

        ActivityModelMapper = new ActivityModelMapper();
        ActivityToUserModelMapper = new ActivityToUserModelMapper();
        SubjectModelMapper = new SubjectModelMapper();
        SubjectToUserModelMapper = new SubjectToUserModelMapper();
        UserModelMapper = new UserModelMapper();


        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<StudyTrackerDbContext> DbContextFactory { get; }

    protected ActivityEntityMapper ActivityEntityMapper { get; }
    protected ActivityToUserMapper ActivityToUserMapper { get; }
    protected SubjectEntityMapper SubjectEntityMapper { get; }
    protected SubjectToUserMapper SubjectToUserMapper { get; }
    protected UserEntityMapper UserEntityMapper { get; }

    protected IActivityModelMapper ActivityModelMapper { get; }
    protected IActivityToUserModelMapper ActivityToUserModelMapper { get; }
    protected ISubjectModelMapper SubjectModelMapper { get; }
    protected ISubjectToUserModelMapper SubjectToUserModelMapper { get; }
    protected IUserModelMapper UserModelMapper { get; }

    protected UnitOfWorkFactory UnitOfWorkFactory { get; }


    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}