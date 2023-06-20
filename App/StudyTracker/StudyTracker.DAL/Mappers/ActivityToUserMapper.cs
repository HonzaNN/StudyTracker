using StudyTracker.DAL.Entities;

namespace StudyTracker.DAL.Mappers;

public class ActivityToUserMapper : IEntityMapper<ActivityToUserEntity>
{
    public void MapToExistingEntity(ActivityToUserEntity existingEntity, ActivityToUserEntity newEntity)
    {
        existingEntity.Id = newEntity.Id;
        if (existingEntity.Activity != null)
        {
            existingEntity.Activity = newEntity.Activity;
            existingEntity.ActivityId = newEntity.ActivityId;
        }

        if (existingEntity.User != null)
        {
            existingEntity.User = newEntity.User;
            existingEntity.UserId = newEntity.UserId;
        }
    }
}