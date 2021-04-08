using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using GPSNotebook.Extensions;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.PinService;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    // --- contains lots of tmp stuff just for debug or trying  ---
    // --- will be replaced with useful code in further commits ---

    public class MapTabViewModel : TabViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;

        public MapTabViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;

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

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

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
                // лагает при переходе между табами
                // если вызывать этот метод в конструкторе, то при первом запуске не лагает
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
