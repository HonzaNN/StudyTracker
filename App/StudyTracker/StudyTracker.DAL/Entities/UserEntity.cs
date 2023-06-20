namespace StudyTracker.DAL.Entities
{
    public record UserEntity : IEntity
    {
        public required string Name { get; set; }

        public required string Surname { get; set; }

        public string? ImageUri { get; set; }

        public ICollection<SubjectToUserEntity> Subjects { get; set; } = new List<SubjectToUserEntity>();

        public ICollection<ActivityToUserEntity> Activities { get; set; } = new List<ActivityToUserEntity>();

        public Guid? TeacherToSubjectId { get; set; }

        public Guid? CreatorOfActivityId { get; set; }


        public required Guid Id { get; set; }
    }
}