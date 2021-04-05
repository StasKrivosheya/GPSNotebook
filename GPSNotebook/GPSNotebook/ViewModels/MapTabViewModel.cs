using System.Collections.Generic;
using System.Collections.ObjectModel;
using GPSNotebook.Models;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.PinService;
using GPSNotebook.Views;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using GoogleMap = Xamarin.Forms.GoogleMaps.Map;

namespace GPSNotebook.ViewModels
{
    // --- contains lots of tmp stuff just for debug or trying  ---
    // --- will be replaced with useful code in further commits ---

    public class MapTabViewModel : ViewModelBase
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
            NavigationService.SelectTabAsync(nameof(PinsListTab));
            UpdatePinsCollection();
        }

        #region -- Public properties --

        private ObservableCollection<Pin> _pinsList;

        public ObservableCollection<Pin> PinsList
        {
            get => _pinsList;
            set => SetProperty(ref _pinsList, value);
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

            PinsList = new ObservableCollection<Pin>(pins);
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        #region -- TMP SandBox --

        private GoogleMap _mainMap;
        public GoogleMap MainMap
        {
            get => _mainMap;
            set => SetProperty(ref _mainMap, value);
        }

        async void NavigateToLocation()
        {
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            MainMap.MyLocationEnabled = true;
            var position = await Geolocation.GetLocationAsync();
            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                Distance.FromMiles(1)));
        }
        #endregion
    }
}
