using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GPSNotebook.Extensions;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.Permissions;
using GPSNotebook.Services.PinService;
using Plugin.Permissions;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class MapTabViewModel : TabViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPermissionsService _permissionsService;

        public MapTabViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService,
            IPermissionsService permissionsService)
            : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;
            _permissionsService = permissionsService;

            IsActiveChanged += OnTabActivated;
        }

        #region -- Public properties --

        private ObservableCollection<PinViewModel> _pinsCollection;
        public new ObservableCollection<PinViewModel> PinsCollection
        {
            get => _pinsCollection;
            set => SetProperty(ref _pinsCollection, value);
        }

        private CameraPosition _myCameraPosition;
        public CameraPosition MyCameraPosition
        {
            get => _myCameraPosition;
            set => SetProperty(ref _myCameraPosition, value);
        }
        #endregion

        private bool _wasLocationGranted;
        public bool WasLocationGranted
        {
            get => _wasLocationGranted;
            set => SetProperty(ref _wasLocationGranted, value);
        }

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            WasLocationGranted = await _permissionsService.TryGetPermissionAsync<LocationPermission>();

            if (parameters.TryGetValue(nameof(CameraPosition), out CameraPosition cameraPosition))
            {
                MyCameraPosition = cameraPosition;
            }
        }

        #endregion

        #region -- Private Helpers --

        private void OnTabActivated(object sender, EventArgs e)
        {
            if (IsActive)
            {
                UpdatePinsCollection();
            }
        }

        private async void UpdatePinsCollection()
        {
            var pinModels = await _pinService.GetPinsListAsync(
                pin => pin.UserId == _authorizationService.GetCurrentUserId);

            List<PinViewModel> pins = new List<PinViewModel>();

            foreach (var pinModel in pinModels)
            {
                pins.Add(pinModel.ToPinViewModel());
            }

            PinsCollection = new ObservableCollection<PinViewModel>(pins);
        }

        #endregion
    }
}
