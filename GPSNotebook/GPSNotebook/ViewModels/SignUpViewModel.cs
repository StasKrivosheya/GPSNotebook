using System.ComponentModel;
using Acr.UserDialogs;
using GPSNotebook.Models;
using GPSNotebook.Resources;
using GPSNotebook.Services.UserService;
using GPSNotebook.Validators;
using Prism.Commands;
using Prism.Navigation;

namespace GPSNotebook.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        public SignUpViewModel(INavigationService navigationService,
            IUserService userService) :
            base(navigationService)
        {
            Title = Resource.SignUpTitle;

            _userService = userService;
        }

        #region --- Public Properties

        private DelegateCommand _signUpCommand;
        public DelegateCommand SignUpCommand =>
            _signUpCommand ?? (_signUpCommand = new DelegateCommand(ExecuteSignUpCommand));

        private bool _isButtonEnabled;
        public bool IsEnabled
        {
            get => _isButtonEnabled;
            private set => SetProperty(ref _isButtonEnabled, value);
        }

        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Password) ||
                args.PropertyName == nameof(ConfirmPassword) ||
                args.PropertyName == nameof(Mail) ||
                args.PropertyName == nameof(Name))
            {
                UpdateSignUpButtonState();
            }
        }

        #endregion

        #region --- Command Handlers ---

        private async void ExecuteSignUpCommand()
        {
            if (Password == ConfirmPassword)
            {
                Mail = Mail.Trim();
                Name = Name.Trim();

                var loginValid = StringValidator.Validate(Mail, StringValidator.Mail);
                var passwordValid = StringValidator.Validate(Password, StringValidator.Password);

                if (!loginValid)
                {
                    UserDialogs.Instance.Alert(Resource.InvalidMailMessage,
                        Resource.InvalidMailTitle,
                        Resource.OkText);
                }
                else if (!passwordValid)
                {
                    UserDialogs.Instance.Alert(Resource.InvalidPasswordMessage,
                        Resource.InvalidPasswordTitle,
                        Resource.OkText);
                }
                else
                {
                    var userModel = new UserModel
                    {
                        Mail = Mail,
                        Name = Name,
                        Password = Password
                    };

                    var result = await _userService.InsertUserAsync(userModel);

                    if (result == -1)
                    {
                        UserDialogs.Instance.Alert(Resource.RegisterFailedMessage,
                            Resource.RegisterFailedTitle,
                            Resource.OkText);
                    }
                    else
                    {
                        var parameters = new NavigationParameters { { nameof(Mail), Mail } };
                        await NavigationService.GoBackAsync(parameters);
                    }
                }
            }
            else
            {
                UserDialogs.Instance.Alert(Resource.DifferentPasswordError,
                    Resource.DifferentPasswordError,
                    Resource.OkText);
            }
        }

        #endregion

        #region --- Private Helpers ---

        private void UpdateSignUpButtonState()
        {
            var areAllEntriesFilled = !string.IsNullOrEmpty(_mail) &&
                                      !string.IsNullOrEmpty(_password) &&
                                      !string.IsNullOrEmpty(_confirmPassword);

            IsEnabled = areAllEntriesFilled;
        }

        #endregion
    }
}
