using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.Permissions;
using GPSNotebook.Services.PinService;
using GPSNotebook.Services.Repository;
using GPSNotebook.Services.Settings;
using GPSNotebook.Services.UserService;
using GPSNotebook.ViewModels;
using GPSNotebook.Views;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using Xamarin.Forms;

namespace GPSNotebook
{
    public partial class App : PrismApplication
    {
        private IAuthorizationService _authorizationService;
        private IAuthorizationService AuthorizationService =>
            _authorizationService ?? (_authorizationService = Container.Resolve<IAuthorizationService>());

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        #region -- Overrides --

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Services
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IUserService>(Container.Resolve<UserService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<IPermissionsService>(Container.Resolve<PermissionsService>());

            containerRegistry.RegisterPopupNavigationService();

            // Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<MapTab, MapTabViewModel>();
            containerRegistry.RegisterForNavigation<PinsListTab, PinsListTabViewModel>();
            containerRegistry.RegisterForNavigation<AddEditPinPage, AddEditPinPageViewModel>();
            containerRegistry.RegisterForNavigation<PinInfoPopupPage, PinInfoPopupPageViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (AuthorizationService.IsAuthorized)
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(HomePage)}?selectedTab={nameof(MapTab)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SignInPage)}");
            }
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
