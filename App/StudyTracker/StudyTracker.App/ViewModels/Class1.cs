using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserDetailViewModel : ViewModelBase, IRecipient<LoadMainPageMessage>, IRecipient<UserEditedMessage>
{
    private readonly INavigationService navigationService;
    private readonly IUserFacade userFacade;


    public UserDetailViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.userFacade = userFacade;

        this.navigationService = navigationService;
    }

    public Guid Id { get; set; }

    public UserDetailModel User { get; set; } = UserDetailModel.Empty;

    public ActivityListModel Activity1 { get; set; } = ActivityListModel.Empty;
    public ActivityListModel Activity2 { get; set; } = ActivityListModel.Empty;

    public async void Receive(LoadMainPageMessage message)
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

        if (Activities.Count <= 0) return;
        Activity1 = Activities[0];
        if (Activities.Count > 1) Activity2 = Activities[1];
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        User = await userFacade.GetAsync(Id);
        GetUpcommingActivities(User.Activities);
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
            { nameof(EditUserPageViewModel.Id), Id }
        });
    }
}