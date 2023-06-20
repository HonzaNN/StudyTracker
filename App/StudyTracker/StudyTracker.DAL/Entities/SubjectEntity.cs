namespace StudyTracker.DAL.Entities
{
    public record SubjectEntity : IEntity
    {
        public required string Name { get; set; }

        public required string Shortcut { get; set; }


        public ICollection<SubjectToUserEntity> Users { get; set; } = new List<SubjectToUserEntity>();

        public ICollection<ActivityEntity> Activities { get; set; } = new List<ActivityEntity>();


        public required Guid TeacherId { get; set; }


        public required Guid Id { get; set; }
    }
}