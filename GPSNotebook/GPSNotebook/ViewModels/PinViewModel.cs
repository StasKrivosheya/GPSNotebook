using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class PinViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Latitude { get; private set; }

        public string Longitude { get; private set; }

        public bool IsFavorite { get; set; }

        public Position Position
        {
            get => new Position(double.Parse(Latitude), double.Parse(Longitude));
            set
            {
                Latitude = value.Latitude.ToString();
                Longitude = value.Longitude.ToString();
            }
        }
    }
}
