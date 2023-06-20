namespace StudyTracker.BL.Models;

public record ActivityToUserDetailModel : ModelBase
{
    public Guid? ActivityId { get; set; }
    public Guid? UserId { get; set; }

    public static ActivityToUserDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        ActivityId = Guid.Empty,
        UserId = Guid.Empty
    };
}