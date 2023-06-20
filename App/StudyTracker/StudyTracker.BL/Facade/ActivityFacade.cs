using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.UnitOfWork;

namespace StudyTracker.BL.Facade;

public class ActivityFacade : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>,
    IActivityFacade
{

    private readonly IActivityModelMapper _activityModelMapper;
    public ActivityFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        IActivityModelMapper activityModelMapper,
        IActivityModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper) 
    {
        _activityModelMapper = modelMapper;

    }

    public virtual async Task<ObservableCollection<ActivityListModel>> GetSubjectActivities(Guid subjectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail) && !string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail)
                .Include(IncludesNavigationPathDetail2);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }
        else if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail2))
        {
            query = query.Include(IncludesNavigationPathDetail2);
        }
        ObservableCollection<ActivityEntity> entities = query
            .Where(e => e.SubjectId == subjectId)
            .ToObservableCollection();

        ObservableCollection<ActivityListModel> models = entities
            .Select(entity => _activityModelMapper.MapToListModel(entity))
            .ToObservableCollection();

        return models;

    }

    protected override string IncludesNavigationPathDetail =>
        $"{nameof(ActivityEntity.Users)}.{nameof(ActivityToUserEntity.User)}";
}