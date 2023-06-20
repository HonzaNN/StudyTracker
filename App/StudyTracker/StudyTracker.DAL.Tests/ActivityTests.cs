using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL.Entities;
using Xunit.Abstractions;

namespace StudyTracker.DAL.Tests
{
    public class ActivityTests : DbTestBase
    {
        public ActivityTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task NewActivity_Add_Activity()
        {
            var subject = new SubjectEntity()
            {
                Id = Guid.Parse("26DBC198-FA34-4210-8C12-960F7C3281BF"),
                Name = "name",
                Shortcut = "sho",
                TeacherId = Guid.Parse("99562927-0A8F-4134-9E37-62BE08BE6017"), 
            };

            studyTrackerDbContextSUT.Subjects.Add(subject);

            var activity = new ActivityEntity()
            {
                Id = Guid.Parse("C0DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                Name = "Test",
                Type = Common.ActivityTypeEntity.Lecture,
                StartDate = new DateTime(2023, 4, 9, 12, 30, 0),
                EndDate = new DateTime(2023, 4, 9, 14, 30, 0),
                ActivityCreatorId = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                State = Common.ActivityStateEntity.Upcoming,
                SubjectId = subject.Id
                
            };

            studyTrackerDbContextSUT.Activity.Add(activity);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Activity
                .SingleAsync(i => i.Id == activity.Id);

            Assert.NotNull(actualEntities);
            Assert.Equal(actualEntities.Name, activity.Name);
            Assert.Equal(actualEntities.Type, activity.Type);
            Assert.Equal(actualEntities.StartDate, activity.StartDate);
            Assert.Equal(actualEntities.EndDate, activity.EndDate);
            Assert.Equal(actualEntities.ActivityCreatorId, activity.ActivityCreatorId);
            Assert.Equal(actualEntities.State, activity.State);
            Assert.Equal(actualEntities.Id, activity.Id);
        }

        [Fact]
        public async Task Delete_Activity()
        {
            // Arrange
            var subject = new SubjectEntity()
            {
                Name = "vut fit subject",
                Shortcut = "VFS",
                TeacherId = Guid.Parse("90562927-0A8F-4134-9E37-62BE08BE6017"),
                Id = Guid.Parse("DDB65741-7027-4F52-8179-79EC3FBB85DD")
            };

            studyTrackerDbContextSUT.Subjects.Add(subject);

            var activity = new ActivityEntity()
            {
                Id = Guid.Parse("C1DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                Name = "Test",
                Type = Common.ActivityTypeEntity.Lecture,
                StartDate = new DateTime(2023, 4, 9, 12, 30, 0),
                EndDate = new DateTime(2023, 4, 9, 14, 30, 0),
                ActivityCreatorId = Guid.Parse("C5DE45D7-04A0-4E8D-AC7F-BF5CFDFB0EFC"),
                State = Common.ActivityStateEntity.Upcoming,
                SubjectId = subject.Id
               
            };


            studyTrackerDbContextSUT.Activity.Add(activity);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            studyTrackerDbContextSUT.Activity.Remove(activity);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            // Assert

            Assert.Null(await studyTrackerDbContextSUT.Users.FindAsync(activity.Id));
        }

        [Fact]
        public async Task NewUser_Update_User()
        {
            // Arrange
            var subject = new SubjectEntity()
            {
                Name = "vut fit subject",
                Shortcut = "VFS",
                TeacherId = Guid.Parse("90562927-0A8F-4134-9E37-62B111116017"),
                Id = Guid.Parse("DDB65741-7027-4F52-8179-71111FBB85DD")
            };

            studyTrackerDbContextSUT.Subjects.Add(subject);

            var activity = new ActivityEntity()
            {
                Id = Guid.Parse("C1DE45D7-6000-4E8D-AC7F-BF5CFDFB0EFC"),
                Name = "Test",
                Type = Common.ActivityTypeEntity.Lecture,
                StartDate = new DateTime(2023, 4, 9, 12, 30, 0),
                EndDate = new DateTime(2023, 4, 9, 14, 30, 0),
                ActivityCreatorId = Guid.Parse("C5DE45D7-04A0-0000-AC7F-BF5CFDFB0EFC"),
                State = Common.ActivityStateEntity.Upcoming,
                SubjectId = subject.Id
            };

            // Act
            studyTrackerDbContextSUT.Activity.Add(activity);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            activity.State = Common.ActivityStateEntity.Done;
            studyTrackerDbContextSUT.Activity.Update(activity);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            // Assert
            var updatedActivity = await studyTrackerDbContextSUT.Activity.FindAsync(activity.Id);
            Assert.Equal(updatedActivity.State, activity.State);
        }
    }
}