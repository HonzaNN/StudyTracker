using System.Collections.ObjectModel;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;


namespace StudyTracker.BL.Facade.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<ObservableCollection<ActivityListModel>> GetSubjectActivities(Guid subjectId);
}