using System.ComponentModel;
using Acr.UserDialogs;
using GPSNotebook.Resources;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.UserService;
using GPSNotebook.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNotebook.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        #region --- Constructors ---

        public SignInViewModel(INavigationService navigationService,
            IUserService userService,
            IAuthorizationService authorizationService) :
            base(navigationService)
        {
            Title = Resource.SignInTitle;

            _userService = userService;

            _authorizationService = authorizationService;
        }

        #endregion

        #region --- Public Properties ---

        private DelegateCommand _navigateCommand;
        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(ExecuteNavigateCommand));

        private DelegateCommand _signInCommand;
        public DelegateCommand SignInCommand =>
            _signInCommand ?? (_signInCommand = new DelegateCommand(ExecuteSignInCommand));

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetProperty(ref _isButtonEnabled, value);
        }

        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(Mail), out string mail))
            {
                Mail = mail;
                Password = string.Empty;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Password) ||
                args.PropertyName == nameof(Mail))
            {
                UpdateSignInButtonState();
            }
        }

        #endregion

        #region --- Command Handlers ---

        private async void ExecuteNavigateCommand()
        {
            await NavigationService.NavigateAsync(nameof(SignUpPage));
        }

        private async void ExecuteSignInCommand()
        {
            var query = await _userService.GetItemAsync(u =>
                u.Mail.Equals(Mail) && u.Password.Equals(Password));

            if (query != null)
            {
                _authorizationService.Authorize(query.Id, query.Mail);

                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}" +
                                                      $"/{nameof(HomePage)}");
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(Resource.SignInErrorMessage,
                    Resource.SignInErrorTitle,
                    Resource.OkText);
                Password = string.Empty;
            }
        }

        #endregion

        #region --- Private Helpers ---

        private void UpdateSignInButtonState()
        {
            IsButtonEnabled = !string.IsNullOrEmpty(Mail) &&
                              !string.IsNullOrEmpty(Password);
        }

        #endregion
    }
}
