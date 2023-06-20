using StudyTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.Seeds;


public static class ActivityToUserSeeds
{
    public static readonly ActivityToUserEntity EmptyActivityToUserEntity = new()
    {
        Id = default,
        UserId = default,
        ActivityId = default,
    };

    public static readonly ActivityToUserEntity Item1 = new()
    {
        Id = Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        UserId = UserSeeds.User1.Id,
        ActivityId = ActivitySeeds.Activity1.Id,

        Activity = ActivitySeeds.Activity1,
        User = UserSeeds.User1
    };

    public static readonly ActivityToUserEntity Item2 = new()
    {
        Id = Guid.Parse("98944e77-16ba-5d7b-011b-fe6ace99dbd9"),
        UserId = UserSeeds.User2.Id,
        ActivityId = ActivitySeeds.Activity1.Id,

        Activity = ActivitySeeds.Activity1,
        User = UserSeeds.User2
    };

    public static readonly ActivityToUserEntity Item3 = new()
    {
        Id = Guid.Parse("65611c44-83ab-2a4a-788d-cb3fab66bfb6"),
        UserId = UserSeeds.User1.Id,
        ActivityId = ActivitySeeds.Activity2.Id,

        Activity = ActivitySeeds.Activity2,
        User = UserSeeds.User1
    };

    public static readonly ActivityToUserEntity Item4 = new()
    {
        Id = Guid.Parse("65611c44-03ab-2a4a-788d-cb3fab66bfb6"),
        UserId = UserSeeds.User2.Id,
        ActivityId = ActivitySeeds.Activity2.Id,

        Activity = ActivitySeeds.Activity2,
        User = UserSeeds.User2
    };

    public static readonly ActivityToUserEntity Item5 = new()
    {
        Id = Guid.Parse("54500033-720f-1a3f-677f-ac2eab55cfe5"),
        UserId = UserSeeds.User3.Id,
        ActivityId = ActivitySeeds.Activity3.Id,

        Activity = ActivitySeeds.Activity3,
        User = UserSeeds.User3
    };


    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityToUserEntity>().HasData(
            Item1 with { User = null, Activity = null },
            Item2 with { User = null, Activity = null },
            Item3 with { User = null, Activity = null },
            Item4 with { User = null, Activity = null },
            Item5 with { User = null, Activity = null }
        );
}