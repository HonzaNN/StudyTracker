using StudyTracker.DAL.Common;

namespace StudyTracker.DAL.Entities;

public record ActivityEntity : IEntity
{
    public required string Name { get; set; }
    public required ActivityTypeEntity Type { get; set; }

    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }


    public required ActivityStateEntity State { get; set; }

    public Guid ActivityCreatorId { get; set; }
    public Guid SubjectId { get; set; }
    public ICollection<ActivityToUserEntity> Users { get; set; } = new List<ActivityToUserEntity>();
    public SubjectEntity? Subject { get; set; } = null!;


    public required Guid Id { get; set; }
}