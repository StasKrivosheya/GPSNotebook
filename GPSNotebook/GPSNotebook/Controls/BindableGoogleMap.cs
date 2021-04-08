using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using GPSNotebook.Extensions;
using GPSNotebook.ViewModels;
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
            PinsSource = new ObservableCollection<PinViewModel>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;

            // temp solution, Permission Service will be implemented soon
            AskLocationPermissionAsync();

            MapClicked += OnMapClicked;
            //    will be needed soon
            // CameraIdled += OnCameraIdled;
            // MapLongClicked += OnMapLongClicked;
            // PinClicked += OnPinClicked;

            MyLocationEnabled = true;
            UiSettings.MyLocationButtonEnabled = true;
            UiSettings.CompassEnabled = true;
        }

        #region -- Public Properties --

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
            propertyName: nameof(PinsSource),
            returnType: typeof(ObservableCollection<PinViewModel>),
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

        public static readonly BindableProperty PinMarkerProperty = BindableProperty.Create(
            propertyName: nameof(PinMarker),
            returnType: typeof(PinViewModel),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

        public ObservableCollection<PinViewModel> PinsSource
        {
            get => (ObservableCollection<PinViewModel>)GetValue(PinsSourceProperty);
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

        public PinViewModel PinMarker
        {
            get => (PinViewModel)GetValue(PinMarkerProperty);
            set => SetValue(PinMarkerProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(PinsSource))
            {
                if (PinsSource != null)
                {
                    Pins.Clear();

                    foreach (PinViewModel pin in PinsSource)
                    {
                        Pins.Add(pin.ToPin());
                    }

                }
            }

            if (propertyName == nameof(PinMarker))
            {
                var pinToShow = PinMarker.ToPin();
                pinToShow.IsVisible = true;

                Pins.Clear();
                Pins.Add(pinToShow);

                CameraPosition cameraPosition = new CameraPosition(PinMarker.Position, DEFAULT_CAMERA_ZOOM);
                var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                AnimateCamera(cameraUpdate);
            }
        }

        #endregion

        #region -- Private Helpers --

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableGoogleMap;
            var newPinsSource = newValue as ObservableCollection<PinViewModel>;

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
            UpdatePinsSource(this, sender as IEnumerable<PinViewModel>);
        }

        private static void UpdatePinsSource(GoogleMap bindableMap, IEnumerable<PinViewModel> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
                bindableMap.Pins.Add(pin.ToPin());
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (MapClickedCommand != null)
            {
                if (MapClickedCommand.CanExecute(e.Point))
                {
                    MapClickedCommand.Execute(e.Point);
                }
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
