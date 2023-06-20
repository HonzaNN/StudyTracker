using Microsoft.EntityFrameworkCore;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;
using StudyTracker.DAL.Mappers;
using StudyTracker.DAL.UnitOfWork;

namespace StudyTracker.BL.Facade;

public class SubjectFacade : FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>,
    ISubjectFacade
{
    private readonly ISubjectModelMapper _subjectModelMapper;
    public SubjectFacade(
        IUnitOfWorkFactory unitOfWorkFactory,
        ISubjectModelMapper modelMapper)
        : base(unitOfWorkFactory, modelMapper)
    {
        _subjectModelMapper = modelMapper;
    }
    public virtual async Task<SubjectDetailModel?> GetSubjectWithActivitiesAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<SubjectEntity> query = uow.GetRepository<SubjectEntity, SubjectEntityMapper>().Get();
        query = query.Include(s => s.Activities);

        SubjectEntity? entity = await query
            .Include(s => s.Activities)
            .SingleOrDefaultAsync(e => e.Id == id);

        return entity is null
            ? null
            : _subjectModelMapper.MapToDetailModel(entity);
    }
    
    protected override string IncludesNavigationPathDetail =>
        $"{nameof(SubjectEntity.Users)}.{nameof(SubjectToUserEntity.User)}";

    protected override string IncludesNavigationPathDetail2 =>
        $"{nameof(SubjectEntity.Activities)}";


}