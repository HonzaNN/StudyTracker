using StudyTracker.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudyTracker.DAL.Seeds;

public static class SubjectSeeds
{
    public static readonly SubjectEntity EmptySubjectEntity = new()
    {
        Id = default,
        Name = default,
        Shortcut = default,
        TeacherId = default,
    };

    public static readonly SubjectEntity Subject1 = new()
    {
        Id = Guid.Parse("06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
        Name = "Seminar C#",
        Shortcut = "ICS",
        TeacherId = UserSeeds.User1.Id,
        
};

    public static readonly SubjectEntity Subject2 = new()
    {
        Id = Guid.Parse("16a8a2cf-ea03-4095-a3e4-aa0291fe9c74"),
        Name = "Zaklady umelej inteligencie",
        Shortcut = "IZU",
        TeacherId = UserSeeds.User3.Id,
        
};


    static SubjectSeeds()
    { 
        Subject1.Activities.Add(ActivitySeeds.Activity1);
        Subject1.Activities.Add(ActivitySeeds.Activity2);
        Subject2.Activities.Add(ActivitySeeds.Activity3);

        Subject1.Users.Add(SubjectToUserSeeds.Item1);
        Subject1.Users.Add(SubjectToUserSeeds.Item2);
        Subject2.Users.Add(SubjectToUserSeeds.Item3);

       
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectEntity>().HasData(
            Subject1 with { Users = Array.Empty<SubjectToUserEntity>(), Activities = Array.Empty<ActivityEntity>()},
            Subject2 with { Users = Array.Empty<SubjectToUserEntity>(), Activities = Array.Empty<ActivityEntity>()}
        );
}