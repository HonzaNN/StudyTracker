using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(CurrentUserId), nameof(CurrentUserId))]
public partial class MainPageViewModel : ViewModelBase, IRecipient<LoadMainPageMessage>, IRecipient<UserEditedMessage>,
    IRecipient<SubjectEditedMessage>, IRecipient<ActivityEditedMessage>, IRecipient<ActivityCreatedMessage>,
    IRecipient<ActivityDeleteMessage>
{
    private readonly INavigationService navigationService;
    private readonly IUserFacade userFacade;


    public MainPageViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.userFacade = userFacade;

        this.navigationService = navigationService;
    }

    public Guid CurrentUserId { get; set; }

    public UserDetailModel User { get; set; } = UserDetailModel.Empty;

    public ICollection<SubjectListModel> Subjects { get; set; } = null!;

    public ActivityListModel Activity1 { get; set; } = ActivityListModel.Empty;
    public bool Activity1Visible { get; set; }
    public ActivityListModel Activity2 { get; set; } = ActivityListModel.Empty;
    public bool Activity2Visible { get; set; }
    public bool MissingActivities { get; set; }

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

    public async void Receive(LoadMainPageMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(SubjectEditedMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserEditedMessage message)
    {
        await LoadDataAsync();
    }

    private void GetUpcommingActivities(ObservableCollection<ActivityListModel> Activities)
    {
        var SortedActivities = Activities.ToList().OrderBy(x => x.EndDate);
        Activities = new ObservableCollection<ActivityListModel>();
        foreach (var activity in SortedActivities)
            if (activity.EndDate > DateTime.Now)
                Activities.Add(activity);

        if (Activities.Count <= 0)
        {
            MissingActivities = true;
            return;
        }

        Activity1 = Activities[0];
        Activity1Visible = true;
        if (Activities.Count > 1)
        {
            Activity2Visible = true;
            Activity2 = Activities[1];
        }
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        User = await userFacade.GetAsync(CurrentUserId);
        if (User != null)
        {
            GetUpcommingActivities(User.Activities);
            Subjects = User.Subjects;
        }

        await base.LoadDataAsync();
    }

    [RelayCommand]
    private async Task LogOutAsync()
    {
        await navigationService.GoToAsync("//LoginPage");
    }

    [RelayCommand]
    private async Task EditUserAsync()
    {
        await navigationService.GoToAsync("//LoginPage/MainPage/EditUserPage", new Dictionary<string, object?>
        {
            { nameof(EditUserPageViewModel.Id), CurrentUserId }
        });
    }

    [RelayCommand]
    public async Task GoToActivityAsync(ActivityListModel activity)
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/ActivityPage", new Dictionary<string, object?>
        {
            { nameof(ActivityPageViewModel.UserId), CurrentUserId },
            { nameof(ActivityPageViewModel.ActivityId), activity.Id },
            { nameof(ActivityPageViewModel.SubjectId), activity.SubjectId }
        });
    }

    [RelayCommand]
    private async Task GoToSubjectListAsync()
    {
        await navigationService.GoToAsync<SubjectListViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectListViewModel.CurrentUserId), CurrentUserId }
        });
    }

    [RelayCommand]
    private async Task GoToSubjectAsync(Guid subjectID)
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage", new Dictionary<string, object>
        {
            { nameof(SubjectPageViewModel.SubjectId), subjectID },
            { nameof(SubjectPageViewModel.CurrentUserId), CurrentUserId }
        });
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync<CreateSubjectPageViewModel>(new Dictionary<string, object>
        {
            { nameof(CreateSubjectPageViewModel.CurrentUserId), CurrentUserId }
        });
    }
}