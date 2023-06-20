using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL.Entities;
using Xunit.Abstractions;

namespace StudyTracker.DAL.Tests
{
    public class UserTests : DbTestBase
    {
        public UserTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task NewUser_Add_User()
        {
            var user = new UserEntity()
            {
                Id = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
                Name = "Lukasz",
                Surname = "Pycz",
                ImageUri = "URL"
            };

            
            studyTrackerDbContextSUT.Users.Add(user);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntities = await dbx.Users.SingleAsync(i => i.Id == user.Id);
            Assert.NotNull(actualEntities);
            Assert.Equal(user.Name, actualEntities.Name);
            Assert.Equal(user.Surname, actualEntities.Surname);
            Assert.Equal(user.Id, actualEntities.Id);
            Assert.Equal(user.ImageUri, actualEntities.ImageUri);
        }

        [Fact]
        public async Task NewUser_Added_Correct_values()
        {
            var user = new UserEntity()
            {
                Id = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EF0"),
                Name = "Elena",
                Surname = "Ivanova",
            };


            studyTrackerDbContextSUT.Users.Add(user);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            Assert.Equal("Elena", user.Name);
            Assert.Equal("Ivanova", user.Surname);
        }


        [Fact]
        public async Task NewUser_Delete_User()
        {
            var user = new UserEntity()
            {
                Id = Guid.Parse("C5DE0000-64A0-4E8D-AC7F-BF5CFDFB0EF0"),
                Name = "Elena",
                Surname = "Ivanova",
            };


            studyTrackerDbContextSUT.Users.Add(user);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            Assert.NotNull(user);

            studyTrackerDbContextSUT.Users.Remove(user);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            Assert.Null(await studyTrackerDbContextSUT.Users.FindAsync(user.Id));
        }

        [Fact]
        public async Task NewUser_Update_User()
        {
            var user = new UserEntity()
            {
                Id = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EF2"),
                Name = "Elena",
                Surname = "Ivanova",
            };


            studyTrackerDbContextSUT.Users.Add(user);
            await studyTrackerDbContextSUT.SaveChangesAsync();

            user.Surname = "Petrova";
            studyTrackerDbContextSUT.Users.Update(user);
            await studyTrackerDbContextSUT.SaveChangesAsync();


            // Assert
            var updatedUser = await studyTrackerDbContextSUT.Users.FindAsync(user.Id);
            Assert.Equal(updatedUser.Surname, user.Surname);
        }
    }
}