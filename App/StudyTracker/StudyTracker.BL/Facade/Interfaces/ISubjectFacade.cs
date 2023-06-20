using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Facade.Interfaces;

public interface ISubjectFacade : IFacade<SubjectEntity, SubjectListModel, SubjectDetailModel>
{
    Task<SubjectDetailModel?> GetSubjectWithActivitiesAsync(Guid id);
}