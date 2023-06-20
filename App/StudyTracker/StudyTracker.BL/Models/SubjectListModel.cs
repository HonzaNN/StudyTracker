namespace StudyTracker.BL.Models;

public record SubjectListModel : ModelBase
{
    public required string Name { get; set; }

    public required string Shortcut { get; set; }

    public static SubjectListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        //idk bout the name
        Name = string.Empty,
        Shortcut = string.Empty
    };
}