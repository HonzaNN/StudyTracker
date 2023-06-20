using System.Collections.ObjectModel;

namespace StudyTracker.BL.Models;

public record UserDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? ImageUri { get; set; }

    public ObservableCollection<SubjectListModel> Subjects { get; set; } = new();
    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();

    public static UserDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Surname = string.Empty,
        ImageUri = string.Empty
    };
}