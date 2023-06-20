using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(ActivityId), nameof(ActivityId))]
[QueryProperty(nameof(UserId), nameof(UserId))]
[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class ActivityPageViewModel : ViewModelBase, IRecipient<ActivityEditedMessage>
{
    private readonly IActivityFacade activityFacade;
    private readonly IActivityToUserFacade activityToUserFacade;
    private readonly INavigationService navigationService;


    public ActivityPageViewModel(
        IActivityFacade activityFacade,
        IActivityToUserFacade activityToUserFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.activityFacade = activityFacade;
        this.activityToUserFacade = activityToUserFacade;
        this.navigationService = navigationService;
    }

    public Guid ActivityId { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;
    public Guid SubjectId { get; set; } = Guid.Empty;
    public ActivityDetailModel Activity { get; set; }
    public bool IsJoined { get; set; }
    public bool IsNotJoined { get; set; }
    public bool JoinDisable { get; set; }

    public async void Receive(ActivityEditedMessage message)
    {
        await LoadDataAsync();
    }


    private bool HasUserFreeTime()
    {
        return activityToUserFacade.HasFreeTime(UserId, Activity.StartDate, Activity.EndDate).Result;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activity = await activityFacade.GetAsync(ActivityId);
        SubjectId = Activity.SubjectId;

        if (await activityToUserFacade.IsJoined(Activity.Id, UserId))
        {
            IsJoined = true;
            IsNotJoined = false;
            JoinDisable = false;
        }
        else
        {
            IsJoined = false;
            if (HasUserFreeTime())
            {
                IsNotJoined = true;
                JoinDisable = false;
            }
            else
            {
                IsNotJoined = false;
                JoinDisable = true;
            }
        }
    }

    [RelayCommand]
    public async Task GoBackAsync()
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage", new Dictionary<string, object>
        {
            { nameof(SubjectPageViewModel.SubjectId), SubjectId },
            { nameof(SubjectPageViewModel.CurrentUserId), UserId }
        });
    }

    [RelayCommand]
    public async Task GoToEditActivityAsync()
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/ActivityPage/EditActivityPage",
            new Dictionary<string, object>
            {
                { nameof(EditActivityPageViewModel.ActivityId), ActivityId },
                { nameof(EditActivityPageViewModel.UserId), UserId },
                { nameof(EditActivityPageViewModel.SubjectId), SubjectId }
            });
    }

    [RelayCommand]
    private async Task JoinAsync()
    {
        var activityToUser = new ActivityToUserDetailModel
        {
            ActivityId = ActivityId,
            UserId = UserId
        };
        await activityToUserFacade.SaveAsync(activityToUser, UserId);
        messengerService.Send(new ActivityEditedMessage
        {
            Id = ActivityId
        });

        messengerService.Send(new UserEditedMessage
        {
            Id = UserId
        });

        IsJoined = true;
        IsNotJoined = false;
    }

    [RelayCommand]
    private async Task LeaveAsync()
    {
        await activityToUserFacade.DeleteAsync(await activityToUserFacade.GetID(Activity.Id, UserId));
        messengerService.Send(new ActivityEditedMessage
        {
            Id = ActivityId
        });

        messengerService.Send(new UserEditedMessage
        {
            Id = UserId
        });

        IsJoined = false;
        IsNotJoined = true;
    }
}