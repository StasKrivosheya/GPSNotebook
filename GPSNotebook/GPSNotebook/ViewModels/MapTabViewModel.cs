using System.Collections.ObjectModel;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using GoogleMap = Xamarin.Forms.GoogleMaps.Map;

namespace GPSNotebook.ViewModels
{
    // --- contains lots of tmp stuff just for debug or trying  ---
    // --- will be replaced with useful code in further commits ---

    public class MapTabViewModel : ViewModelBase
    {
        public MapTabViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            PinsList = new ObservableCollection<Pin>
            {
                new Pin {Label = "Pin1", Position = new Position(0, 0)},
                new Pin {Label = "Rome", Position = new Position(41.902782, 12.496366)}
            };
        }

        #region -- Public properties --

        private ObservableCollection<Pin> _pinsList;

        public ObservableCollection<Pin> PinsList
        {
            get => _pinsList;
            set => SetProperty(ref _pinsList, value);
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
