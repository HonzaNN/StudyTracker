using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Common;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(CurrentUserId), nameof(CurrentUserId))]
[QueryProperty(nameof(CurrentSubjectId), nameof(CurrentSubjectId))]
public partial class CreateActivityPageViewModel : ViewModelBase
{
    private readonly IActivityFacade activityFacade;
    private readonly IActivityModelMapper activityModelMapper;
    private readonly IActivityToUserFacade activityToUserFacade;


    private readonly INavigationService navigationService;
    private readonly ISubjectFacade subjectFacade;
    private readonly ISubjectModelMapper subjectModelMapper;
    private readonly IUserFacade userFacade;

    private readonly IUserModelMapper userModelMapper;

    public CreateActivityPageViewModel(
        IActivityFacade activityFacade,
        ISubjectFacade subjectFacade,
        IUserFacade userFacade,
        IActivityToUserFacade activityToUserFacade,
        ISubjectModelMapper subjectModelMapper,
        IUserModelMapper userModelMapper,
        IActivityModelMapper activityModelMapper,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.activityFacade = activityFacade;
        this.subjectFacade = subjectFacade;
        this.userFacade = userFacade;
        this.activityToUserFacade = activityToUserFacade;
        this.subjectModelMapper = subjectModelMapper;
        this.userModelMapper = userModelMapper;
        this.activityModelMapper = activityModelMapper;
        this.navigationService = navigationService;
    }

    public Guid CurrentUserId { get; set; }
    public Guid CurrentSubjectId { get; set; }

    public ActivityDetailModel ActivityDetail { get; set; } = ActivityDetailModel.Empty;

    public DateTime MinDateTime { get; set; } = new(1900, 01, 01);
    public DateTime MaxDateTime { get; set; } = new(2100, 01, 01);
    public DateTime SelectedStartDateTime { get; set; } = DateTime.Now;
    public DateTime SelectedEndDateTime { get; set; } = DateTime.Now;
    public TimeSpan SelectedStartTimeSpan { get; set; } = DateTime.Now.TimeOfDay;
    public TimeSpan SelectedEndTimeSpan { get; set; } = DateTime.Now.TimeOfDay;

    public ObservableCollection<ActivityTypeEntity> TypeCollection { get; } =
        new(Enum.GetValues(typeof(ActivityTypeEntity)).Cast<ActivityTypeEntity>());

    public ObservableCollection<ActivityStateEntity> StateCollection { get; } =
        new(Enum.GetValues(typeof(ActivityStateEntity)).Cast<ActivityStateEntity>());

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
    }

    [RelayCommand]
    public async Task CreateActivity(ActivityDetailModel activity)
    {
        activity.StartDate = SelectedStartDateTime.Date + SelectedStartTimeSpan;
        activity.EndDate = SelectedEndDateTime.Date + SelectedEndTimeSpan;
        if (activity.Name == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a name for the activity", "OK");
            return;
        }

        if (activityToUserFacade.HasFreeTime(CurrentUserId, activity.StartDate, activity.EndDate).Result == false)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You have already selected activity in this time",
                "OK");
            return;
        }

        var activityToUserDetail = ActivityToUserDetailModel.Empty;
        activityToUserDetail.ActivityId = activity.Id;
        activityToUserDetail.UserId = CurrentUserId;

        activity.ActivityCreatorId = CurrentUserId;
        activity.SubjectId = CurrentSubjectId;

        var user = await userFacade.GetAsync(activity.ActivityCreatorId);
        var subject = await subjectFacade.GetAsync(activity.SubjectId);

        var user_list = userModelMapper.MapToListModel(userModelMapper.MapToEntity(user));
        var subject_list = subjectModelMapper.MapToListModel(subjectModelMapper.MapToEntity(subject));

        var Activity_Entity = activityModelMapper.MapToEntity(activity);
        var ActivityList = activityModelMapper.MapToListModel(Activity_Entity);
        var ActivityDetail = activityModelMapper.MapToDetailModel(Activity_Entity);

        user.Activities.Add(ActivityList);
        subject.Activities.Add(ActivityList);

        if (activity.StartDate > activity.EndDate)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Invalid date or time", "OK");
            return;
        }


        await userFacade.UpdateAsync(user with { Activities = default, Subjects = default });
        messengerService.Send(new UserEditedMessage { Id = user.Id });
        await subjectFacade.UpdateAsync(subject with { Activities = default, Users = default });
        messengerService.Send(new SubjectEditedMessage { Id = subject.Id });

        ActivityDetail.Users.Add(user_list);


        try
        {
            await activityFacade.SaveAsync(ActivityDetail with { Users = default, SubjectId = CurrentSubjectId });
            await activityToUserFacade.SaveAsync(activityToUserDetail, CurrentUserId);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                var innerException = ex.InnerException;
            }
        }

        messengerService.Send(new ActivityCreatedMessage { ActivtityId = ActivityDetail.Id });

        await navigationService.GoToAsync<SubjectPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectPageViewModel.CurrentUserId), CurrentUserId },
            { nameof(SubjectPageViewModel.SubjectId), CurrentSubjectId }
        });
    }

    [RelayCommand]
    public async Task DiscardAsync()
    {
        ActivityDetail = ActivityDetailModel.Empty;
        await navigationService.GoToAsync<SubjectPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectPageViewModel.CurrentUserId), CurrentUserId },
            { nameof(SubjectPageViewModel.SubjectId), CurrentSubjectId }
        });
    }
}