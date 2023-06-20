using CommunityToolkit.Mvvm.Input;
using StudyTracker.App.Messages;
using StudyTracker.App.Services;
using StudyTracker.BL.Facade.Interfaces;
using StudyTracker.BL.Mappers.Interface;
using StudyTracker.BL.Models;
using StudyTracker.DAL.Entities;

namespace StudyTracker.App.ViewModels;

[QueryProperty(nameof(CurrentUserId), nameof(CurrentUserId))]
public partial class CreateSubjectPageViewModel : ViewModelBase
{
    private readonly INavigationService navigationService;
    private readonly ISubjectFacade subjectFacade;
    private readonly ISubjectModelMapper subjectModelMapper;
    private readonly ISubjectToUserFacade subjectToUserFacade;
    private readonly IUserFacade userFacade;
    private readonly IUserModelMapper userModelMapper;

    public CreateSubjectPageViewModel(ISubjectFacade subjectFacade,
        IUserFacade userFacade,
        ISubjectToUserFacade subjectToUserFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        ISubjectModelMapper subjectModelMapper,
        IUserModelMapper userModelMapper)
        : base(messengerService)
    {
        this.subjectFacade = subjectFacade;
        this.userFacade = userFacade;
        this.subjectToUserFacade = subjectToUserFacade;
        this.navigationService = navigationService;
        this.subjectModelMapper = subjectModelMapper;
        this.userModelMapper = userModelMapper;
    }

    public Guid CurrentUserId { get; set; }
    public SubjectDetailModel SubjectDetail { get; set; } = SubjectDetailModel.Empty;

    public SubjectEntity Subject_Entity { get; set; }

    public SubjectToUserDetailModel? subjectToUser { get; } = new();

    public SubjectListModel SubjectList { get; set; } = SubjectListModel.Empty;
    public UserListModel user_list { get; set; } = UserListModel.Empty;
    public IList<UserListModel> AllUsers { get; set; } = null!;
    public UserListModel SelectedUser { get; set; } = UserListModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        AllUsers = new List<UserListModel>(await userFacade.GetAsync());
    }

    [RelayCommand]
    public async Task CreateSubjectAsync(SubjectDetailModel subject)
    {
        if (subject.Name == "" || subject.Shortcut == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter subject name and shortcut", "OK");
            return; 
        }

        if (SelectedUser == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter the teacher's name", "OK");
            return;
        }

        subjectToUser.SubjectId = subject.Id;


        subject.TeacherId = SelectedUser.Id;

        var user = await userFacade.GetAsync(subject.TeacherId);
        user_list = userModelMapper.MapToListModel(userModelMapper.MapToEntity(user));


        Subject_Entity = subjectModelMapper.MapToEntity(subject);

        SubjectList = subjectModelMapper.MapToListModel(Subject_Entity);
        SubjectDetail = subjectModelMapper.MapToDetailModel(Subject_Entity);


        if (subject.Name == "" || subject.Shortcut == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter subject name and shortcut", "OK");
            return; 
        }

        await userFacade.UpdateAsync(user with { Activities = default, Subjects = default });
        
        messengerService.Send(new UserEditedMessage { Id = user.Id });


        await subjectFacade.SaveAsync(SubjectDetail with { Users = default, Activities = default });

        messengerService.Send(new SubjectCreatedMessage { SubjectId = subject.Id });
        user = await userFacade.GetAsync(SelectedUser.Id);
        SubjectDetail = await subjectFacade.GetAsync(subject.Id);

        try
        {
            await subjectToUserFacade.SaveAsync(subjectToUser, SelectedUser.Id);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                var innerException = ex.InnerException;
            }
        }

        await navigationService.GoToAsync<SubjectListViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectListViewModel.CurrentUserId), CurrentUserId }
        });
    }


    [RelayCommand]
    public async Task DiscardAsync()
    {
        SubjectDetail = SubjectDetailModel.Empty;
        await navigationService.GoToAsync<SubjectListViewModel>(new Dictionary<string, object?>
        {
            { nameof(SubjectListViewModel.CurrentUserId), CurrentUserId }
        });
    }
}