using StudyTracker.DAL.Common;

namespace StudyTracker.BL.Models;

public record ActivityListModel : ModelBase
{
    public required ActivityTypeEntity Type { get; set; }
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required ActivityStateEntity State { get; set; }
    public required Guid ActivityCreatorId { get; set; }
    public required Guid SubjectId { get; set; }

    public static ActivityListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Type = ActivityTypeEntity.None,
        Name = string.Empty,
        StartDate = DateTime.Now,
        EndDate = DateTime.Now,
        State = ActivityStateEntity.None,
        ActivityCreatorId = Guid.Empty,
        SubjectId = Guid.Empty
    };
}