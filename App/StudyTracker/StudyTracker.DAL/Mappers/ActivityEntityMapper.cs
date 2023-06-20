using StudyTracker.DAL.Entities;

namespace StudyTracker.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        existingEntity.Type = newEntity.Type;
        existingEntity.Name = newEntity.Name;
        existingEntity.StartDate = newEntity.StartDate;
        existingEntity.EndDate = newEntity.EndDate;
        existingEntity.State = newEntity.State;
        existingEntity.Users = newEntity.Users;
        existingEntity.ActivityCreatorId = newEntity.ActivityCreatorId;
        existingEntity.SubjectId = newEntity.SubjectId;
    }
}