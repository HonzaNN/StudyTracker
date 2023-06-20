namespace StudyTracker.BL.Models;

public record SubjectToUserDetailModel : ModelBase
{
    public Guid? SubjectId { get; set; }
    public Guid? UserId { get; set; }

    public static SubjectToUserDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        SubjectId = Guid.Empty,
        UserId = Guid.Empty
    };
}