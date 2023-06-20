using StudyTracker.DAL.Common;
using StudyTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity EmptyActivityEntity = new()
    {
        Id = default,
        Name = "",
        ActivityCreatorId = default,
        Type = default,
        StartDate = default,
        EndDate = default,
        State = default,
        
    };
    public static readonly ActivityEntity Activity1 = new()
    {
        Id = Guid.Parse("df935095-8709-4040-a2bb-b6f97cb416dc"),
        Name = "ICS Study Session",
        Type = ActivityTypeEntity.StudySession,
        StartDate = new DateTime(2023, 05, 12, 10, 00, 00),
        EndDate = new DateTime(2023, 05, 12, 10, 00, 00),
        ActivityCreatorId = default,
        State = ActivityStateEntity.Upcoming,
        SubjectId = SubjectSeeds.Subject1.Id
    };

    public static readonly ActivityEntity Activity2 = new()
    {
        Id = Guid.Parse("ef824984-7698-3939-a1bb-b5f86cb305dc"),
        Name = "ICS Practical Class",
        Type = ActivityTypeEntity.PracticalClass,
        StartDate = new DateTime(2023, 05, 11, 14, 00, 00),
        EndDate = new DateTime(2023, 06, 11, 15, 50, 00),
        ActivityCreatorId = UserSeeds.User2.Id,
        State = ActivityStateEntity.Upcoming,
        SubjectId = SubjectSeeds.Subject1.Id
    };

    public static readonly ActivityEntity Activity3 = new()
    {
        Id = Guid.Parse("cf602762-5476-1717-a9bb-b3f64cb183dc"),
        Name = "IZU Study Session",
        Type = ActivityTypeEntity.StudySession,
        StartDate = new DateTime(2023, 05, 10, 8, 00, 00),
        EndDate = new DateTime(2023, 05, 10, 10, 00, 00),
        ActivityCreatorId = UserSeeds.User3.Id,
        State = ActivityStateEntity.Upcoming,
        SubjectId = SubjectSeeds.Subject2.Id
    };

    static ActivitySeeds()
    {
        Activity1.Users.Add(ActivityToUserSeeds.Item1);
        Activity1.Users.Add(ActivityToUserSeeds.Item2);
        Activity2.Users.Add(ActivityToUserSeeds.Item3);
        Activity2.Users.Add(ActivityToUserSeeds.Item4);
        Activity3.Users.Add(ActivityToUserSeeds.Item5);

        Activity1.Subject = SubjectSeeds.Subject1;
        Activity2.Subject = SubjectSeeds.Subject1;
        Activity3.Subject = SubjectSeeds.Subject2;
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityEntity>().HasData(
            Activity1 with { Users = Array.Empty<ActivityToUserEntity>(), Subject = null},
            Activity2 with { Users = Array.Empty<ActivityToUserEntity>(), Subject = null },
            Activity3 with { Users = Array.Empty<ActivityToUserEntity>(), Subject = null }
        );
}