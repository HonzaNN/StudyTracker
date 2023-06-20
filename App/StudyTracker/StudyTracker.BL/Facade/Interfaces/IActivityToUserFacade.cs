using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Facade.Interfaces;

public interface IActivityToUserFacade : IFacadeManyToMany<ActivityToUserEntity, ActivityToUserDetailModel>
{
    Task<List<ActivityToUserEntity>> GetByUserIdAsync(Guid userId);
    Task<List<ActivityToUserEntity>> GetByActivityIdAsync(Guid activityId);
    Task<bool> IsJoined(Guid activityId, Guid userId);
    Task<Guid> GetID(Guid activityId, Guid userId);
    Task<bool> HasFreeTime(Guid userId, DateTime start, DateTime end);
}