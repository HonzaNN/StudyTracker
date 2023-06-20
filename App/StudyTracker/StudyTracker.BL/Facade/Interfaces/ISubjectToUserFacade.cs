using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;


namespace StudyTracker.BL.Facade.Interfaces;

public interface ISubjectToUserFacade : IFacadeManyToMany<SubjectToUserEntity, SubjectToUserDetailModel>

{
    Task<List<SubjectToUserEntity>> GetByUserIdAsync(Guid userId);
    Task<List<SubjectToUserEntity>> GetBySubjectIdAsync(Guid subjectId);
    Task<bool> IsJoined(Guid subjectId, Guid userId);
    Task<Guid> GetId(Guid subjectId, Guid userId);
}