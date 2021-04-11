using System.ComponentModel;
using GPSNotebook.Extensions;
using GPSNotebook.Services.PinService;
using Prism.Mvvm;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class PinViewModel : BindableBase
    {
        public PinViewModel(int id, string pinImagePath, int userId, string name, string description, string latitude,
            string longitude, bool isFavorite, Position position)
        {
            Id = id;
            PinImagePath = pinImagePath;
            UserId = userId;
            Name = name;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            _isFavorite = isFavorite;
            Position = position;
        }

        public PinViewModel()
        {
        }

        #region -- Public Properties --

        public int Id { get; set; }

        public string PinImagePath { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Latitude { get; private set; }

        public string Longitude { get; private set; }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        public Position Position
        {
            get => new Position(double.Parse(Latitude), double.Parse(Longitude));
            set
            {
                Latitude = value.Latitude.ToString();
                Longitude = value.Longitude.ToString();
            }
        }

        #endregion

        #region -- Overrides --

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsFavorite))
            {
                await App.Resolve<IPinService>().UpdatePinAsync(this.ToPinModel());
            }
        }

        #endregion
    }
}
