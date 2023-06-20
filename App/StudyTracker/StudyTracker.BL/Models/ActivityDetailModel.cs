using System.Collections.ObjectModel;
using StudyTracker.DAL.Common;

namespace StudyTracker.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required ActivityStateEntity State { get; set; }
    public required ActivityTypeEntity Type { get; set; }
    public ObservableCollection<UserListModel> Users { get; set; } = new();
    public required Guid ActivityCreatorId { get; set; }
    public required Guid SubjectId { get; set; }


    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        StartDate = DateTime.Now,
        EndDate = DateTime.Now,
        State = ActivityStateEntity.None,
        Type = ActivityTypeEntity.None,
        ActivityCreatorId = Guid.Empty,
        SubjectId = Guid.Empty
    };
}