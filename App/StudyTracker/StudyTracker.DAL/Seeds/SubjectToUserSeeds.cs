using StudyTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.Seeds;

public static class SubjectToUserSeeds
{
    public static readonly SubjectToUserEntity EmptySubjectToUserEntity = new()
    {
        Id = default,
        UserId = default,
        SubjectId = default,

        Subject = default,
        User = default
    };

    public static readonly SubjectToUserEntity Item1 = new()
    {
        Id = Guid.Parse("06a8a2cf-ea03-4095-a3e5-aa0291fe9c75"),
        UserId = UserSeeds.User1.Id,
        SubjectId = SubjectSeeds.Subject1.Id,

        Subject = SubjectSeeds.Subject1,
        User = UserSeeds.User1
    };

    public static readonly SubjectToUserEntity Item2 = new()
    {
        Id = Guid.Parse("17b9B3de-fb14-5106-b4f6-bb13020f0d86"),
        UserId = UserSeeds.User2.Id,
        SubjectId = SubjectSeeds.Subject1.Id,

        Subject = SubjectSeeds.Subject1,
        User = UserSeeds.User2
    };

    public static readonly SubjectToUserEntity Item3 = new()
    {
        Id = Guid.Parse("28b0b4de-fb25-6217-b506-cc2413fc1e97"),
        UserId = UserSeeds.User3.Id,
        SubjectId = SubjectSeeds.Subject1.Id,

        Subject = SubjectSeeds.Subject1,
        User = UserSeeds.User3
    };

    public static readonly SubjectToUserEntity Item4 = new()
    {
        Id = Guid.Parse("40c2c6ea-bc47-8439-c7b8-dd4635ab3a19"),
        UserId = UserSeeds.User3.Id,
        SubjectId = SubjectSeeds.Subject2.Id,

        Subject = SubjectSeeds.Subject2,
        User = UserSeeds.User3
    };

    public static readonly SubjectToUserEntity Item5 = new()
    {
        Id = Guid.Parse("40c2c6ea-bc47-8439-c7b8-dd4635ab3a10"),
        UserId = UserSeeds.User2.Id,
        SubjectId = SubjectSeeds.Subject2.Id,

        Subject = SubjectSeeds.Subject2,
        User = UserSeeds.User2
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectToUserEntity>().HasData(
            Item1 with { Subject = null, User = null },
            Item2 with { Subject = null, User = null },
            Item3 with { Subject = null, User = null },
            Item4 with { Subject = null, User = null },
            Item5 with { Subject = null, User = null }
        );
}