using StudyTracker.App.Models;
using StudyTracker.App.ViewModels;
using StudyTracker.App.Views;

namespace StudyTracker.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//LoginPage", typeof(LoginPageView), typeof(LoginPageViewModel)),
        new("//LoginPage/AddUserPage", typeof(AddUserPageView), typeof(AddUserPage)),
        new("//LoginPage/MainPage", typeof(MainPageView), typeof(MainPageViewModel)),
        new("//LoginPage/MainPage/EditUserPage", typeof(EditUserPageView), typeof(EditUserPageViewModel)),

        new("//SubjectListPage", typeof(SubjectListPageView), typeof(SubjectListViewModel)),
        new("//SubjectListPage/CreateSubjectPage", typeof(CreateSubjectPageView), typeof(CreateSubjectPageViewModel)),
        new("//SubjectListPage/SubjectPage", typeof(SubjectPageView), typeof(SubjectPageViewModel)),
        new("//SubjectListPage/SubjectPage/EditSubjectPage", typeof(EditSubjectPageView),
            typeof(EditSubjectPageViewModel)),
        new("//SubjectListPage/SubjectPage/ActivityPage", typeof(ActivityPageView), typeof(ActivityPageViewModel)),
        new("//SubjectListPage/SubjectPage/ActivityPage/EditActivityPage", typeof(EditActivityPageView),
            typeof(EditActivityPageViewModel)),
        new("//SubjectListPage/SubjectPage/CreateActivityPage", typeof(CreateActivityPageView),
            typeof(CreateActivityPageViewModel))
    };

    public async Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
    {
        await Shell.Current.GoToAsync(route, parameters);
    }

    public bool SendBackButtonPressed()
    {
        return Shell.Current.SendBackButtonPressed();
    }

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel
    {
        return Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
    }
}