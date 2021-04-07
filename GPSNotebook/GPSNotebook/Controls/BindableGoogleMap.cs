using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
//using GoogleMap = Xamarin.Forms.GoogleMaps.Clustering.ClusteredMap;
using GoogleMap = Xamarin.Forms.GoogleMaps.Map;

namespace GPSNotebook.Controls
{
    class BindableGoogleMap : GoogleMap
    {
        #region -- Private Constants --

        private const double DEFAULT_CAMERA_ZOOM = 15d;

        #endregion

        public BindableGoogleMap()
        {
            PinsSource = new ObservableCollection<Pin>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;

            // temp solution, Permission Service will be implemented soon
            AskLocationPermissionAsync();

            MapClicked += OnMapClicked;

            //CameraIdled += OnCameraIdled;
            //MapLongClicked += OnMapLongClicked;
            //PinClicked += OnPinClicked;

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

        public static readonly BindableProperty MapClickedCommandProperty = BindableProperty.Create(
            propertyName: nameof(MapClickedCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(BindableGoogleMap),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValue: null);

        public static readonly BindableProperty TappedPinProperty = BindableProperty.Create(
            propertyName: nameof(TappedPin),
            returnType: typeof(Pin),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

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

        public ICommand MapClickedCommand
        {
            get => (ICommand) GetValue(MapClickedCommandProperty);
            set => SetValue(MapClickedCommandProperty, value);
        }

        public Pin TappedPin
        {
            get => (Pin)GetValue(TappedPinProperty);
            set => SetValue(TappedPinProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(TappedPin))
            {
                Pins.Clear();
                Pins.Add(TappedPin);

                CameraPosition cameraPosition = new CameraPosition(TappedPin.Position, DEFAULT_CAMERA_ZOOM);
                var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                AnimateCamera(cameraUpdate);
            }
        }

        #endregion

        #region -- Private Helpers --

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableGoogleMap;
            var newPinsSource = newValue as ObservableCollection<Pin>;

            if (thisInstance != null && newPinsSource != null)
                UpdatePinsSource(thisInstance, newPinsSource);
        }

        private static async void MyCameraPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BindableGoogleMap thisInstance)
            {
                if (newValue is CameraPosition newCamPos)
                {
                    newCamPos = new CameraPosition(newCamPos.Target, DEFAULT_CAMERA_ZOOM);
                    var cameraUpdate = CameraUpdateFactory.NewCameraPosition(newCamPos);
                    await thisInstance.AnimateCamera(cameraUpdate);
                }
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

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (MapClickedCommand != null)
            {
                //Pins.Clear();

                //Pins.Add(new Pin
                //{
                //    Label = string.Empty,
                //    Position = e.Point
                //});

                MapClickedCommand.Execute(e.Point);
            }
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
