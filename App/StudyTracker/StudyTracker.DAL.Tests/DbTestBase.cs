using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL.Factories;
using StudyTracker.Common.Tests;

namespace StudyTracker.DAL.Tests
{
    public class DbTestBase : IAsyncLifetime
    {
        protected DbTestBase(ITestOutputHelper output)
        {
            XUnitTestOutputConverter converter = new(output);
            Console.SetOut(converter);

            DbContextFactory = new DbContextSqLiteFactory(GetType().FullName!, seedTestingData: false);


            studyTrackerDbContextSUT = DbContextFactory.CreateDbContext();
            //some extensions
        }

        protected IDbContextFactory<StudyTrackerDbContext> DbContextFactory { get; set; }
        protected StudyTrackerDbContext studyTrackerDbContextSUT { get; }


        //constructor

        //destructor

        public async Task InitializeAsync()
        {
            await studyTrackerDbContextSUT.Database.EnsureDeletedAsync();
            await studyTrackerDbContextSUT.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await studyTrackerDbContextSUT.Database.EnsureDeletedAsync();
            await studyTrackerDbContextSUT.DisposeAsync();
        }
    }
}