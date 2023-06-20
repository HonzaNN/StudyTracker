using StudyTracker.DAL.Entities;

namespace StudyTracker.DAL.Mappers;

public class UserEntityMapper : IEntityMapper<UserEntity>
{
    public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        existingEntity.Name = newEntity.Name;
        existingEntity.Surname = newEntity.Surname;

        if (existingEntity.ImageUri != null)
        {
            existingEntity.ImageUri = newEntity.ImageUri;
        }

        if (existingEntity.CreatorOfActivityId != null)
        {
            existingEntity.CreatorOfActivityId = newEntity.CreatorOfActivityId;
        }

        if (existingEntity.TeacherToSubjectId != null)
        {
            existingEntity.TeacherToSubjectId = newEntity.TeacherToSubjectId;
        }

        existingEntity.Activities = newEntity.Activities;
    }
}