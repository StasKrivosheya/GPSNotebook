using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using GoogleMap = Xamarin.Forms.GoogleMaps.Map;

namespace GPSNotebook.ViewModels
{
    class MapTabViewModel
    {
        GoogleMap MainMap = new GoogleMap();
        
        async void ShowLocation()
        {

            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            MainMap.MyLocationEnabled = true;
            var position = await Geolocation.GetLocationAsync();
            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                Distance.FromMiles(1)));
        }
    }
}
