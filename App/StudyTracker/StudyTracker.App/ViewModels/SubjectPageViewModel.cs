using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Common;
using StudyTracker.DAL.Seeds;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
[QueryProperty(nameof(CurrentUserId), nameof(CurrentUserId))]
public partial class SubjectPageViewModel : ViewModelBase, IRecipient<ActivityCreatedMessage>,
    IRecipient<ActivityDeleteMessage>, IRecipient<ActivityEditedMessage>
{
    private readonly IActivityFacade activityFacade;
    private readonly IActivityModelMapper activityModelMapper;
    private readonly INavigationService navigationService;
    private readonly ISubjectFacade subjectFacade;
    private readonly ISubjectToUserFacade subjectToUserFacade;
    private readonly IUserFacade userFacade;


    private ObservableCollection<ActivityListModel> filteredActivities;
    private string selectedDateOption;
    private ActivityStateEntity selectedState;
    private ActivityTypeEntity selectedType;


    public SubjectPageViewModel(
        ISubjectFacade subjectFacade,
        IUserFacade userFacade,
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IActivityModelMapper activityModelMapper,
        IMessengerService messengerService,
        ISubjectToUserFacade subjectToUserFacade)
        : base(messengerService)
    {
        this.subjectFacade = subjectFacade;
        this.userFacade = userFacade;
        this.activityFacade = activityFacade;
        this.activityModelMapper = activityModelMapper;
        this.subjectToUserFacade = subjectToUserFacade;
        this.navigationService = navigationService;
    }

    public Guid SubjectId { get; set; }
    public Guid CurrentUserId { get; set; } = Guid.Empty;
    public SubjectDetailModel Subject { get; set; }
    public UserDetailModel Teacher { get; set; } = UserDetailModel.Empty;

    public bool MissingActivities { get; set; }
    public ObservableCollection<ActivityListModel> Activities { get; set; }
    public ObservableCollection<ActivityListModel> allActivities { get; set; }


    public bool IsJoined { get; set; }
    public bool IsNotJoined { get; set; }

    public async void Receive(ActivityCreatedMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityEditedMessage message)
    {
        await LoadDataAsync();
    }

    public void FilterActivitiesByType(ActivityTypeEntity type)
    {
        selectedType = type;
        ApplyFilters();
    }

    public void FilterActivitiesByState(ActivityStateEntity state)
    {
        selectedState = state;
        ApplyFilters();
    }

    public void FilterActivitiesByDate(string selectedDateOption)
    {
        switch (selectedDateOption)
        {
            case "Most Recent":
                filteredActivities =
                    new ObservableCollection<ActivityListModel>(filteredActivities.OrderByDescending(a => a.StartDate));
                break;
            case "Least Recent":
                filteredActivities =
                    new ObservableCollection<ActivityListModel>(filteredActivities.OrderBy(a => a.StartDate));
                break;
        }

        Activities = filteredActivities;
    }

    private void ApplyFilters()
    {
        var activities = Activities;

        if (selectedType == ActivityTypeEntity.None || selectedState == ActivityStateEntity.None)
            activities = allActivities;

        if (selectedType == ActivityTypeEntity.None && selectedState != ActivityStateEntity.None)
            activities = new ObservableCollection<ActivityListModel>(allActivities.Where(a =>
                a.State == selectedState
            ));

        else if (selectedType != ActivityTypeEntity.None && selectedState == ActivityStateEntity.None)
            activities = new ObservableCollection<ActivityListModel>(allActivities.Where(a =>
                a.Type == selectedType
            ));
        else if (selectedType != ActivityTypeEntity.None && selectedState != ActivityStateEntity.None)
            activities = new ObservableCollection<ActivityListModel>(allActivities.Where(a =>
                a.Type == selectedType &&
                a.State == selectedState
            ));


        filteredActivities = activities;
        if (selectedDateOption != null) FilterActivitiesByDate(selectedDateOption);

        Activities = filteredActivities;
        base.LoadDataAsync();
    }


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Subject = await subjectFacade.GetAsync(SubjectId);
        Subject = await subjectFacade.GetSubjectWithActivitiesAsync(SubjectId);

        Subject.Activities.Add(activityModelMapper.MapToListModel(ActivitySeeds.Activity1));
        await subjectFacade.SaveAsync(Subject with { Activities = default, Users = default });
        Subject = await subjectFacade.GetSubjectWithActivitiesAsync(SubjectId);


        Teacher = await userFacade.GetAsync(Subject.TeacherId);

        Activities = await activityFacade.GetSubjectActivities(SubjectId);
        allActivities = await activityFacade.GetSubjectActivities(SubjectId);


        if (Activities.Count == 0)
            MissingActivities = true;
        else
            MissingActivities = false;

        if (subjectToUserFacade.IsJoined(SubjectId, CurrentUserId).Result)
        {
            IsJoined = true;
            IsNotJoined = false;
        }
        else
        {
            IsJoined = false;
            IsNotJoined = true;
        }
    }


    [RelayCommand]
    public async Task GoBackAsync()
    {
        navigationService.GoToAsync<SubjectListViewModel>(new Dictionary<string, object>
        {
            { nameof(SubjectListViewModel.CurrentUserId), CurrentUserId }
        });
    }

    [RelayCommand]
    public async Task GoToCreateActivityAsync()
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/CreateActivityPage",
            new Dictionary<string, object?>
            {
                { nameof(CreateActivityPageViewModel.CurrentUserId), CurrentUserId },
                { nameof(CreateActivityPageViewModel.CurrentSubjectId), SubjectId }
            });
    }

    [RelayCommand]
    public async Task GoToEditSubjectAsync()
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/EditSubjectPage",
            new Dictionary<string, object>
            {
                { nameof(EditSubjectPageViewModel.SubjectId), SubjectId },
                { nameof(EditSubjectPageViewModel.CurrentUserId), CurrentUserId }
            });
    }

    [RelayCommand]
    public async Task GoToActivityAsync(Guid activityId)
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/ActivityPage", new Dictionary<string, object>
        {
            { nameof(EditActivityPageViewModel.ActivityId), activityId },
            { nameof(EditActivityPageViewModel.SubjectId), SubjectId },
            { nameof(EditActivityPageViewModel.UserId), CurrentUserId }
        });
    }

    [RelayCommand]
    public async Task JoinSubjectAsync()
    {
        SubjectToUserDetailModel subjectToUser = new()
        {
            SubjectId = SubjectId,
            UserId = CurrentUserId
        };
        await subjectToUserFacade.SaveAsync(subjectToUser, CurrentUserId);
        messengerService.Send(new SubjectEditedMessage
        {
            Id = SubjectId
        });
        messengerService.Send(new UserEditedMessage
        {
            Id = CurrentUserId
        });
        await LoadDataAsync();
    }

    [RelayCommand]
    public async Task LeaveSubjectAsync()
    {
        await subjectToUserFacade.DeleteAsync(subjectToUserFacade.GetId(SubjectId, CurrentUserId).Result);
        messengerService.Send(new SubjectEditedMessage
        {
            Id = SubjectId
        });
        messengerService.Send(new UserEditedMessage
        {
            Id = CurrentUserId
        });
        await LoadDataAsync();
    }
}