using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(CurrentUserId), nameof(CurrentUserId))]
public partial class SubjectListViewModel : ViewModelBase, IRecipient<SubjectCreatedMessage>
{
    private readonly INavigationService navigationService;
    private readonly ISubjectFacade subjectFacade;

    public string ImagePath;

    public SubjectListViewModel(
        ISubjectFacade subjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.subjectFacade = subjectFacade;
        this.navigationService = navigationService;
    }

    public Guid CurrentUserId { get; set; }

    public IEnumerable<SubjectListModel> Subjects { get; set; } = null!;

    public async void Receive(SubjectCreatedMessage message)
    {
        await LoadDataAsync();
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Subjects = await subjectFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync<CreateSubjectPageViewModel>(new Dictionary<string, object>
        {
            { nameof(CreateSubjectPageViewModel.CurrentUserId), CurrentUserId }
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
    private async Task GoToMainPageAsync()
    {
        await navigationService.GoToAsync<MainPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(MainPageViewModel.CurrentUserId), CurrentUserId }
        });
    }
}