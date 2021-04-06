using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GoogleMap = Xamarin.Forms.GoogleMaps.Map;

namespace GPSNotebook.Controls
{
    class BindableGoogleMap : GoogleMap
    {
        #region -- Private Constants --

        private const double DEFAULT_CAMERA_ZOOM = 10d;

        #endregion

        public BindableGoogleMap()
        {
            PinsSource = new ObservableCollection<Pin>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;

            // temp solution, Permission Service will be implemented soon
            AskLocationPermissionAsync();

            MyLocationEnabled = true;
            UiSettings.MyLocationButtonEnabled = true;
            UiSettings.CompassEnabled = true;
        }

        #region -- Public Properties --

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
            propertyName: nameof(PinsSource),
            returnType: typeof(ObservableCollection<Pin>),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: PinsSourcePropertyChanged);

        public static readonly BindableProperty MyCameraPositionProperty = BindableProperty.Create(
            propertyName: nameof(MyCameraPosition),
            returnType: typeof(CameraPosition),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: MyCameraPositionPropertyChanged);

        public ObservableCollection<Pin> PinsSource
        {
            get => (ObservableCollection<Pin>)GetValue(PinsSourceProperty);
            set => SetValue(PinsSourceProperty, value);
        }

        public CameraPosition MyCameraPosition
        {
            get => (CameraPosition)GetValue(MyCameraPositionProperty);
            set => SetValue(MyCameraPositionProperty, value);
        }

        #endregion

        #region -- Private Helpers --

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var thisInstance = bindable as BindableGoogleMap;
            var newPinsSource = newValue as ObservableCollection<Pin>;

            if (thisInstance == null ||
                newPinsSource == null)
                return;

            UpdatePinsSource(thisInstance, newPinsSource);
        }

        private static async void MyCameraPositionPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is BindableGoogleMap thisInstance)
            {
                var newCamPos = newvalue as CameraPosition;
                newCamPos = new CameraPosition(newCamPos.Target, DEFAULT_CAMERA_ZOOM);
                var a = CameraUpdateFactory.NewCameraPosition(newCamPos);
                await thisInstance.AnimateCamera(a);
            }
        }

        private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(this, sender as IEnumerable<Pin>);
        }

        private static void UpdatePinsSource(GoogleMap bindableMap, IEnumerable<Pin> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
                bindableMap.Pins.Add(pin);
        }

        /// <summary>
        /// Temp method until Permission Service Implemented
        /// </summary>
        private Task<PermissionStatus> AskLocationPermissionAsync()
        {
            return Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        #endregion
    }
}
