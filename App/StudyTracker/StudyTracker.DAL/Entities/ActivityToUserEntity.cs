namespace StudyTracker.DAL.Entities;

public record ActivityToUserEntity : IEntity
{
    public ActivityEntity? Activity { get; set; }

    public UserEntity? User { get; set; }

    public Guid? ActivityId { get; set; }

    public Guid? UserId { get; set; }
    public required Guid Id { get; set; }
}