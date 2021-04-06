using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            UpdatePinsCollection();
        }

        #region -- Public properties --

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

        private async void UpdatePinsCollection()
        {
            var pinModels = await _pinService.GetPinsListAsync(
                pin => pin.UserId == _authorizationService.GetCurrentUserId);

            List<Pin> pins = new List<Pin>();

            foreach (var pinModel in pinModels)
            {
                pins.Add(new Pin
                {
                    Position = new Position(double.Parse(pinModel.Latitude), double.Parse(pinModel.Longitude)),
                    IsVisible = pinModel.IsFavorite,
                    Label = pinModel.Name,
                    Address = pinModel.Description
                });
            }

            PinsCollection = new ObservableCollection<Pin>(pins);
        }

        #endregion
    }
}
