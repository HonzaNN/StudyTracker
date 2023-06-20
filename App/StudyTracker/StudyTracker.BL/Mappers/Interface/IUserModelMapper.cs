using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.BL.Mappers.Interface;

public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel>
{
}