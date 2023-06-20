using StudyTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity EmptyUserEntity = new()
    {
        Id = default,
        Name = default,
        Surname = default,
        ImageUri = default
    };

    public static readonly UserEntity User1 = new()
    {
        Id = Guid.Parse("0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        Name = "Lukasz",
        Surname = "Pycz",
        ImageUri =
            @"https://static.vecteezy.com/system/resources/previews/005/544/718/original/profile-icon-design-free-vector.jpg",
    };

    public static readonly UserEntity User2 = new()
    {
        Id = Guid.Parse("0d4fa151-ad81-4d47-a512-4c666166ec6e"),
        Name = "Jan",
        Surname = "Novak",
        ImageUri =
            @"https://static.vecteezy.com/system/resources/previews/005/544/718/original/profile-icon-design-free-vector.jpg",
    };

    public static readonly UserEntity User3 = new()
    {
        Id = Guid.Parse("0d4fa152-ad82-4d48-a513-4c666166ec7e"),
        Name = "Lucia",
        Surname = "Smotlakova",
        ImageUri =
            @"https://static.vecteezy.com/system/resources/previews/005/544/718/original/profile-icon-design-free-vector.jpg",
    };


    static UserSeeds()
    {
        User1.Activities.Add(ActivityToUserSeeds.Item1);
        User1.Activities.Add(ActivityToUserSeeds.Item3);

        User2.Activities.Add(ActivityToUserSeeds.Item2);
        User2.Activities.Add(ActivityToUserSeeds.Item4);

        User3.Activities.Add(ActivityToUserSeeds.Item5);

        User1.Subjects.Add(SubjectToUserSeeds.Item1);
        User2.Subjects.Add(SubjectToUserSeeds.Item2);
        User3.Subjects.Add(SubjectToUserSeeds.Item3);

        User3.Subjects.Add(SubjectToUserSeeds.Item4);
        User2.Subjects.Add(SubjectToUserSeeds.Item5);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserEntity>().HasData(
            User1 with
            {
                Activities = Array.Empty<ActivityToUserEntity>(), Subjects = Array.Empty<SubjectToUserEntity>()
            },
            User2 with
            {
                Activities = Array.Empty<ActivityToUserEntity>(), Subjects = Array.Empty<SubjectToUserEntity>()
            },
            User3 with
            {
                Activities = Array.Empty<ActivityToUserEntity>(), Subjects = Array.Empty<SubjectToUserEntity>()
            }
        );
}