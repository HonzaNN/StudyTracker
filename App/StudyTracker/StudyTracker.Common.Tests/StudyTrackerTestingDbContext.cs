using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Seeds;

namespace StudyTracker.Common.Tests;

public class StudyTrackerTestingDbContext : StudyTrackerDbContext
{
    private readonly bool _seedTestingData;

    public StudyTrackerTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedDemoData: false)
    {
        _seedTestingData = seedTestingData;
    }

    public DbSet<ActivityEntity> Activities { get; set; }
    public DbSet<ActivityToUserEntity> ActivitiesToUsers { get; set; }
    public DbSet<SubjectEntity> Subjects { get; set; }
    public DbSet<SubjectToUserEntity> SubjectsToUsers { get; set; }
    public DbSet<UserEntity> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SubjectEntity>()
            .HasMany(i => i.Activities)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Subjects)
            .WithOne()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UserEntity>()
           .HasMany(i => i.Activities)
           .WithOne()
           .OnDelete(DeleteBehavior.SetNull);


        modelBuilder.Entity<ActivityToUserEntity>()
            .HasOne(i => i.User)
            .WithMany(i => i.Activities)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ActivityToUserEntity>()
            .HasOne(i => i.Activity)
            .WithMany(i => i.Users)
            .HasForeignKey(i => i.ActivityId)
            .OnDelete(DeleteBehavior.SetNull);


        modelBuilder.Entity<SubjectToUserEntity>()
            .HasOne(i => i.User)
            .WithMany(i => i.Subjects)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<SubjectToUserEntity>()
            .HasOne(i => i.Subject)
            .WithMany(i => i.Users)
            .HasForeignKey(i => i.SubjectId)
            .OnDelete(DeleteBehavior.SetNull);

        if (_seedTestingData)
        {
            UserSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
            SubjectSeeds.Seed(modelBuilder);
            ActivityToUserSeeds.Seed(modelBuilder);
            SubjectToUserSeeds.Seed(modelBuilder);
        }
    }
}