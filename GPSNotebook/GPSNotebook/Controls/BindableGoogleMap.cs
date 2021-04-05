using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.Controls
{
    class BindableGoogleMap : Map
    {
        //public BindableGoogleMap()
        //{
        //    PinsSource = new ObservableCollection<Pin>();
        //    PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
        //}

        //#region -- Public Properties --

        //public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
        //    propertyName: nameof(PinsSource),
        //    returnType: typeof(ObservableCollection<Pin>),
        //    declaringType: typeof(BindableGoogleMap),
        //    defaultValue: null);

        //public static readonly BindableProperty MapSpanProperty =
        //    BindableProperty.Create(
        //        propertyName: nameof(MapSpan),
        //        returnType: typeof(MapSpan),
        //        declaringType: typeof(BindableGoogleMap),
        //        defaultValue: null);

        //public ObservableCollection<Pin> PinsSource
        //{
        //    get => (ObservableCollection<Pin>)GetValue(PinsSourceProperty);
        //    set => SetValue(PinsSourceProperty, value);
        //}

        //public MapSpan MapSpan
        //{
        //    get => (MapSpan)GetValue(MapSpanProperty);
        //    set => SetValue(MapSpanProperty, value);
        //}

        //#endregion

        //#region -- Overrides --

        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);

        //    /*if (propertyName == MapSpanProperty.PropertyName)
        //    {
        //        MoveToRegion(MapSpan);
        //    }*/
        //}

        //#endregion

        //#region -- Private Helpers --

        //private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    UpdatePinsSource(this, sender as IEnumerable<Pin>);
        //}

        //private static void UpdatePinsSource(Map bindableMap, IEnumerable<Pin> newSource)
        //{
        //    bindableMap.Pins.Clear();
        //    foreach (var pin in newSource)
        //        bindableMap.Pins.Add(pin);
        //}

        //#endregion
        public BindableGoogleMap()
        {
            PinsSource = new ObservableCollection<Pin>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
            UiSettings.MyLocationButtonEnabled = true;
        }

        public ObservableCollection<Pin> PinsSource
        {
            get { return (ObservableCollection<Pin>)GetValue(PinsSourceProperty); }
            set { SetValue(PinsSourceProperty, value); }
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: nameof(PinsSource),
                                                         returnType: typeof(ObservableCollection<Pin>),
                                                         declaringType: typeof(BindableGoogleMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);


        public MapSpan MapSpan
        {
            get { return (MapSpan)GetValue(MapSpanProperty); }
            set { SetValue(MapSpanProperty, value); }
        }

        public static readonly BindableProperty MapSpanProperty = BindableProperty.Create(
                                                         propertyName: nameof(MapSpan),
                                                         returnType: typeof(MapSpan),
                                                         declaringType: typeof(BindableGoogleMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: MapSpanPropertyChanged);

        private static void MapSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableGoogleMap;
            var newMapSpan = newValue as MapSpan;

            thisInstance?.MoveToRegion(newMapSpan);
        }
        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var thisInstance = bindable as BindableGoogleMap;
            var newPinsSource = newValue as ObservableCollection<Pin>;

            if (thisInstance == null ||
                newPinsSource == null)
                return;

            UpdatePinsSource(thisInstance, newPinsSource);
        }
        private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(this, sender as IEnumerable<Pin>);
        }

        private static void UpdatePinsSource(Map bindableMap, IEnumerable<Pin> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
                bindableMap.Pins.Add(pin);
        }
    }
}
