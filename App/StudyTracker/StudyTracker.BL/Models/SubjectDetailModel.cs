using System.Collections.ObjectModel;

namespace StudyTracker.BL.Models;

public record SubjectDetailModel : ModelBase
{
    public required string Name { get; set; }

    public required string Shortcut { get; set; }

    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();
    public ObservableCollection<UserListModel> Users { get; set; } = new();
    public Guid TeacherId { get; set; }

    public static SubjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Shortcut = string.Empty,
        TeacherId = Guid.Empty,
    };
}