using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers.Interface;

public interface IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
}