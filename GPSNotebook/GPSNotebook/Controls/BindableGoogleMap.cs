using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.Controls
{
    class BindableGoogleMap : Map
    {
        public BindableGoogleMap()
        {
            PinsSource = new ObservableCollection<Pin>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
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
