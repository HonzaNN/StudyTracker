using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL.Entities;
using Xunit.Abstractions;

namespace StudyTracker.DAL.Tests
{
    public class SubjectTests : DbTestBase
    {
        public SubjectTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Add_New_Subject()
        {
            var subject = new SubjectEntity()
            {
                Id = Guid.Parse("C5DE45D0-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                Name = "jazyk c",
                Shortcut = "ijc",
                TeacherId = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC")
            };
            
            

            studyTrackerDbContextSUT.Subjects.Add(subject);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Subjects
                .SingleAsync(i => i.Id == subject.Id);

            Assert.NotNull(actualEntities);
            Assert.Equal(actualEntities.Name, subject.Name);
            Assert.Equal(actualEntities.Shortcut, subject.Shortcut);
            Assert.Equal(actualEntities.TeacherId, subject.TeacherId);
            Assert.Equal(actualEntities.Id, subject.Id);
        }

        [Fact]
        public async Task NewSubject_Delete_Subject()
        {
            var subject = new SubjectEntity()
            {
                Id = Guid.Parse("92653BB5-943E-4F24-ADB8-EADBC21862E6"),
                Name = "jazyk c",
                Shortcut = "ijc",
                TeacherId = Guid.Parse("F1046722-1140-4B48-82F3-AB7486A5FC05")
            };


            studyTrackerDbContextSUT.Subjects.Add(subject);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            Assert.NotNull(subject);

            studyTrackerDbContextSUT.Subjects.Remove(subject);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            Assert.Null(await studyTrackerDbContextSUT.Subjects.FindAsync(subject.Id));
        }

        [Fact]
        public async Task NewSubject_Update_Subject()
        {
            var subject = new SubjectEntity()
            {
                Id = Guid.Parse("92653BB5-0000-4F24-ADB8-EADBC21862E6"),
                Name = "jazyk c",
                Shortcut = "ijc",
                TeacherId = Guid.Parse("F1046722-0000-4B48-82F3-AB7486A5FC05")
            };

            studyTrackerDbContextSUT.Subjects.Add(subject);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            subject.Shortcut = "icj";
            studyTrackerDbContextSUT.Subjects.Update(subject);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            // Assert
            var updatedSubject = await studyTrackerDbContextSUT.Subjects.FindAsync(subject.Id);
            Assert.Equal(updatedSubject.Shortcut, subject.Shortcut);
        }
    }
}