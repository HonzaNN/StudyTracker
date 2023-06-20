using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
[QueryProperty(nameof(CurrentUserId), nameof(CurrentUserId))]
public partial class EditSubjectPageViewModel : ViewModelBase
{
    private readonly IActivityFacade activityFacade;
    private readonly IActivityToUserFacade activityToUserFacade;
    private readonly INavigationService navigationService;
    private readonly ISubjectFacade subjectFacade;
    private readonly ISubjectToUserFacade subjectToUserFacade;
    private readonly IUserFacade userFacade;

    public EditSubjectPageViewModel(
        ISubjectFacade subjectFacade,
        ISubjectToUserFacade subjectToUserFacade,
        IUserFacade userFacade,
        IActivityToUserFacade activityToUserFacade,
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.subjectFacade = subjectFacade;
        this.userFacade = userFacade;
        this.activityFacade = activityFacade;
        this.navigationService = navigationService;
        this.subjectToUserFacade = subjectToUserFacade;
        this.activityToUserFacade = activityToUserFacade;
    }

    public Guid SubjectId { get; set; }
    public SubjectDetailModel Subject { get; set; } = SubjectDetailModel.Empty;

    public UserDetailModel User { get; set; } = UserDetailModel.Empty;
    public List<UserListModel> AllUsers { get; set; } = new();
    public UserListModel SelectedUser { get; set; } = UserListModel.Empty;
    public Guid CurrentUserId { get; set; } = Guid.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Subject = await subjectFacade.GetAsync(SubjectId);
        if (Subject != null) User = await userFacade.GetAsync(Subject.TeacherId);
        AllUsers = new List<UserListModel>(await userFacade.GetAsync());
        SelectedUser = AllUsers.FirstOrDefault(u => u.Id == Subject.TeacherId);
    }

    [RelayCommand]
    private async Task SaveAsync(Guid teacherId)
    {
        teacherId = SelectedUser.Id;
        await subjectFacade.UpdateAsync(Subject with
        {
            Users = default, Activities = default, TeacherId = teacherId
        }); 
        messengerService.Send(new SubjectEditedMessage { Id = Subject.Id });
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage", new Dictionary<string, object?>
        {
            { nameof(SubjectPageViewModel.SubjectId), SubjectId },
            { nameof(SubjectPageViewModel.CurrentUserId), CurrentUserId }
        });
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage", new Dictionary<string, object?>
        {
            { nameof(SubjectPageViewModel.SubjectId), SubjectId },
            { nameof(SubjectPageViewModel.CurrentUserId), CurrentUserId }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        var entities = await subjectToUserFacade.GetBySubjectIdAsync(SubjectId);
        if (Subject != null && entities.Any())
            foreach (var entity in entities)
            {
                await subjectToUserFacade.DeleteAsync(entity.Id);
            }

        var Activities = await activityFacade.GetAsync();
        Activities = new ObservableCollection<ActivityListModel>(Activities.Where(a => a.SubjectId == SubjectId));


        foreach (var activity in Activities)
        {
            var AtU = await activityToUserFacade.GetByActivityIdAsync(activity.Id);
            if (activity != null && AtU.Any())
                foreach (var entity in AtU)
                {
                
                    await activityToUserFacade.DeleteAsync(entity.Id);
                }

        await activityFacade.DeleteAsync(activity.Id);
        }


        await subjectFacade.DeleteAsync(SubjectId);
        messengerService.Send(new SubjectDeleteMessage());
        await navigationService.GoToAsync<SubjectListViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectListViewModel.CurrentUserId), CurrentUserId }
        });
    }
}