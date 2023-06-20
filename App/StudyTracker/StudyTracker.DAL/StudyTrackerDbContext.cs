using Microsoft.EntityFrameworkCore;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Seeds;

namespace StudyTracker.DAL;

public class StudyTrackerDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public StudyTrackerDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
        : base(contextOptions) =>
        _seedDemoData = seedDemoData;

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<SubjectEntity> Subjects => Set<SubjectEntity>();
    public DbSet<ActivityEntity> Activity => Set<ActivityEntity>();
    public DbSet<SubjectToUserEntity> SubjectToUser => Set<SubjectToUserEntity>();

    public DbSet<ActivityToUserEntity> ActivityToUser => Set<ActivityToUserEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActivityEntity>()
            .HasMany(i=>i.Users)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ActivityEntity>()
            .HasOne<SubjectEntity>()
            .WithMany()
            .HasForeignKey(a=>a.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i=>i.Activities)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Subjects)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        

        modelBuilder.Entity<SubjectEntity>()
            .HasMany(i => i.Users)
            .WithOne()
            .HasForeignKey(a => a.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SubjectEntity>()
            .HasMany(i => i.Activities)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ActivityToUserEntity>()
            .HasOne(i => i.User)
            .WithMany(i => i.Activities)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ActivityToUserEntity>()
            .HasOne(i => i.Activity)
            .WithMany(i => i.Users)
            .HasForeignKey(i => i.ActivityId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<SubjectToUserEntity>()
            .HasOne(i => i.User)
            .WithMany(i => i.Subjects)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<SubjectToUserEntity>()
            .HasOne(i => i.Subject)
            .WithMany(i => i.Users)
            .HasForeignKey(i => i.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);


        if (_seedDemoData)
        { 
            
            ActivityToUserSeeds.Seed(modelBuilder);
            SubjectToUserSeeds.Seed(modelBuilder);
            UserSeeds.Seed(modelBuilder);
            
            ActivitySeeds.Seed(modelBuilder);
            SubjectSeeds.Seed(modelBuilder);

        }
    }
}