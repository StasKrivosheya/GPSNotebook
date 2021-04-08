using GPSNotebook.Resources;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNotebook.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;

        public HomePageViewModel(INavigationService navigationService,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            Title = Resource.HomePageTitle;

            _authorizationService = authorizationService;
        }

        #region -- Public Properties --

        private DelegateCommand _logOutCommand;
        public DelegateCommand LogOutCommand =>
            _logOutCommand ?? (_logOutCommand = new DelegateCommand(ExecuteLogOutCommand));

        #endregion

        #region -- Private Helpers --

        private void ExecuteLogOutCommand()
        {
            _authorizationService.UnAuthorize();

            NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
        }

        #endregion
    }
}
