using StudyTracker.DAL.Entities;

namespace StudyTracker.DAL.Mappers;

public class SubjectEntityMapper : IEntityMapper<SubjectEntity>
{
    public void MapToExistingEntity(SubjectEntity existingEntity, SubjectEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        existingEntity.Name = newEntity.Name;
        existingEntity.Shortcut = newEntity.Shortcut;
        existingEntity.Users = newEntity.Users;
        existingEntity.Activities = newEntity.Activities;
        existingEntity.TeacherId = newEntity.TeacherId;
    }
}