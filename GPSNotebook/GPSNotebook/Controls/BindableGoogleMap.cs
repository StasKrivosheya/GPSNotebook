using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using GPSNotebook.Extensions;
using GPSNotebook.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GoogleMap = Xamarin.Forms.GoogleMaps.Clustering.ClusteredMap;

namespace GPSNotebook.Controls
{
    class BindableGoogleMap : GoogleMap
    {
        public BindableGoogleMap()
        {
            PinsSource = new ObservableCollection<PinViewModel>();
            PinsSource.CollectionChanged += OnPinsSourceCollectionChanged;

            UiSettings.MyLocationButtonEnabled = true;
            UiSettings.CompassEnabled = true;

            MapClicked += OnMapClicked;
        }

        ~BindableGoogleMap()
        {
            MapClicked -= OnMapClicked;
            PinsSource.CollectionChanged -= OnPinsSourceCollectionChanged;
        }

        #region -- Public Properties --

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
            propertyName: nameof(PinsSource),
            returnType: typeof(ObservableCollection<PinViewModel>),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty MyCameraPositionProperty = BindableProperty.Create(
            propertyName: nameof(MyCameraPosition),
            returnType: typeof(CameraPosition),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty MapClickedCommandProperty = BindableProperty.Create(
            propertyName: nameof(MapClickedCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(BindableGoogleMap),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

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

        protected override async void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(PinsSource))
            {
                if (PinsSource != null)
                {
                    UpdatePinsSource(PinsSource);
                }
            }

            if (propertyName == nameof(MyCameraPosition))
            {
                var newCamPos = new CameraPosition(MyCameraPosition.Target, MyCameraPosition.Zoom);
                var cameraUpdate = CameraUpdateFactory.NewCameraPosition(newCamPos);

                await AnimateCamera(cameraUpdate);
            }

            if (propertyName == nameof(PinMarker))
            {
                Pins.Clear();

                var pinToShow = PinMarker.ToPin();

                if (pinToShow != null)
                {
                    pinToShow.IsVisible = true;

                    Pins.Add(pinToShow);

                    var cameraPosition = new CameraPosition(PinMarker.Position, Constants.DEFAULT_CAMERA_ZOOM);
                    var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                    await AnimateCamera(cameraUpdate);
                }
            }
        }

        #endregion

        #region -- Private Helpers --

        private void UpdatePinsSource(IEnumerable<PinViewModel> newSource)
        {
            Pins.Clear();

            foreach (var pin in newSource)
            {
                Pins.Add(pin.ToPin());
            }
        }

        private void OnPinsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(sender as IEnumerable<PinViewModel>);
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            SelectedPin = null;

            if (MapClickedCommand != null &&
                MapClickedCommand.CanExecute(e.Point))
            {
                MapClickedCommand.Execute(e.Point);
            }
        }

        #endregion
    }
}
