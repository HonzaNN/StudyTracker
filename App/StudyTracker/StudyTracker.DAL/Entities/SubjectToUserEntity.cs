namespace StudyTracker.DAL.Entities
{
    public record SubjectToUserEntity : IEntity
    {
        public SubjectEntity? Subject { get; set; }

        public Guid SubjectId { get; set; }
        public UserEntity? User { get; set; }
        public Guid UserId { get; set; }
        public required Guid Id { get; set; }
    }
}