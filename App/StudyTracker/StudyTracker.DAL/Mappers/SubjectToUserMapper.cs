using StudyTracker.DAL.Entities;

namespace StudyTracker.DAL.Mappers;

public class SubjectToUserMapper : IEntityMapper<SubjectToUserEntity>
{
    public void MapToExistingEntity(SubjectToUserEntity existingEntity, SubjectToUserEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        if (existingEntity.Subject != null)
        {
            existingEntity.Subject = newEntity.Subject;
            existingEntity.SubjectId = newEntity.SubjectId;
        }

        if (existingEntity.User != null)
        {
            existingEntity.User = newEntity.User;
            existingEntity.UserId = newEntity.UserId;
        }
    }
}