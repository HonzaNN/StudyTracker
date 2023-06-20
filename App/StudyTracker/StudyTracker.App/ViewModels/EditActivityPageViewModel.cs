using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Common;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(ActivityId), nameof(ActivityId))]
[QueryProperty(nameof(UserId), nameof(UserId))]
[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class EditActivityPageViewModel : ViewModelBase
{
    private readonly IActivityFacade activityFacade;
    private readonly IActivityToUserFacade activityToUserFacade;
    private readonly INavigationService navigationService;

    public EditActivityPageViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IActivityToUserFacade activityToUserFacade,
        IMessengerService messengerService)
        : base(messengerService)
    {
        this.activityFacade = activityFacade;
        this.navigationService = navigationService;
        this.activityToUserFacade = activityToUserFacade;
    }

    public Guid ActivityId { get; set; }
    public ActivityDetailModel Activity { get; set; } = ActivityDetailModel.Empty;

    public ObservableCollection<ActivityTypeEntity> TypeCollection { get; } =
        new(Enum.GetValues(typeof(ActivityTypeEntity)).Cast<ActivityTypeEntity>());

    public ObservableCollection<ActivityStateEntity> StateCollection { get; } =
        new(Enum.GetValues(typeof(ActivityStateEntity)).Cast<ActivityStateEntity>());


    public Guid UserId { get; set; } = Guid.Empty;
    public Guid SubjectId { get; set; } = Guid.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activity = await activityFacade.GetAsync(ActivityId);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await activityFacade.UpdateAsync(Activity with {Users = default});
        messengerService.Send(new ActivityEditedMessage { Id = Activity.Id  });
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/ActivityPage", new Dictionary<string, object?>
        {
            { nameof(ActivityPageViewModel.ActivityId), ActivityId },
            { nameof(ActivityPageViewModel.UserId), UserId },
            { nameof(ActivityPageViewModel.SubjectId), SubjectId }
        });
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await navigationService.GoToAsync("//SubjectListPage/SubjectPage/ActivityPage", new Dictionary<string, object?>
        {
            { nameof(ActivityPageViewModel.ActivityId), ActivityId },
            { nameof(ActivityPageViewModel.UserId), UserId },
            { nameof(ActivityPageViewModel.SubjectId), SubjectId }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        var AtU = await activityToUserFacade.GetByActivityIdAsync(ActivityId);
        if (Activity != null && AtU.Any())
            foreach (var entity in AtU)
                
                await activityToUserFacade.DeleteAsync(entity.Id);
        

        await activityFacade.DeleteAsync(ActivityId);
        messengerService.Send(new ActivityDeleteMessage());
        await navigationService.GoToAsync<SubjectPageViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectPageViewModel.CurrentUserId), UserId },
            { nameof(SubjectPageViewModel.SubjectId), SubjectId }
        });
    }
}